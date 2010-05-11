using System;
using System.Reflection;
using System.Windows.Threading;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

namespace ContactManager.Aspects
{
    [Serializable]
    [ProvideAspectRole( StandardRoles.Threading )]
    public sealed class DispatchAttribute : MethodInterceptionAspect
    {
        public DispatcherPriority Priority { get; set; }

        public bool Async { get; set; }

        public DispatchAttribute()
            : this( DispatcherPriority.Normal, false )
        {
        }

        public DispatchAttribute( DispatcherPriority priority, bool async )
        {
            this.Async = async;
            this.Priority = priority;
        }

        public override bool CompileTimeValidate( MethodBase method )
        {
            if ( !method.IsStatic )
            {
                // If the method is not static, it must be declared in a type
                // derived from DispatcherObject.
                if ( !typeof(DispatcherObject).IsAssignableFrom( method.DeclaringType ) )
                {
                    Message.Write( SeverityType.Error,
                                   "CM00001",
                                   "Cannot apply the [Dispatch] custom attribute to instance methods of type {0} " +
                                   "because it is not derived from DispatcherObject.",
                                   method.DeclaringType.FullName );
                    return false;
                }
            }
            else
            {
                // If the method is static, the type of its first parameter must be deried
                // from DispatcherObject.
                ParameterInfo[] parameters = method.GetParameters();
                if ( parameters.Length < 1 || !typeof(DispatcherObject).IsAssignableFrom( parameters[0].ParameterType ) )
                {
                    Message.Write( SeverityType.Error,
                                   "CM00002",
                                   "Cannot apply the [Dispatch] custom attribute to static method {0} " +
                                   "because the first parameter is not derived from DispatcherObject.",
                                   method.DeclaringType.FullName );
                    return false;
                }
            }


            return true;
        }

        public override void OnInvoke( MethodInterceptionArgs eventArgs )
        {
            DispatcherObject dispatcherObject = (DispatcherObject) (eventArgs.Instance ?? eventArgs.Arguments.GetArgument( 0 ));


            if ( dispatcherObject == null || dispatcherObject.CheckAccess() )
            {
                // We are already in the GUI thread. Proceed.
                eventArgs.Proceed();
            }
            else
            {
                if ( this.Async )
                {
                    // Invoke the target method asynchronously (don't wait).
                    dispatcherObject.Dispatcher.BeginInvoke( this.Priority,
                                                             new Action( eventArgs.Proceed ) );
                }
                else
                {
                    // Invoke the target method synchronously.  
                    dispatcherObject.Dispatcher.Invoke(
                        this.Priority,
                        new Action( eventArgs.Proceed ) );
                }
            }
        }
    }
}