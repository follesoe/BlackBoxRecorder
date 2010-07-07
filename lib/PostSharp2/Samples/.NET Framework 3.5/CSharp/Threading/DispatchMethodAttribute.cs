using System;
using System.Reflection;
using System.Windows.Threading;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Serialization;
using PostSharp.Extensibility;

namespace Threading
{
    [MethodInterceptionAspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
    public sealed class DispatchMethodAttribute : MethodInterceptionAspect
    {
        private DispatcherPriority priority = DispatcherPriority.Normal;

        public bool Async { get; set; }

        public DispatcherPriority Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public override bool CompileTimeValidate( MethodBase method )
        {
            if ( method.IsStatic )
            {
                Message.Write( SeverityType.Error, "THREAD01", "Cannot apply DispatchMethodAttribute to method {0}.{1}: the method is static.",
                               method.DeclaringType.Name, method.Name );
                return false;
            }
            else if ( typeof(DispatcherObject).IsAssignableFrom( method.DeclaringType ) )
            {
                Message.Write( SeverityType.Error, "THREAD02",
                               "Cannot apply DispatchMethodAttribute to methods if type {0}: the type is not derived from DispatcherObject.",
                               method.DeclaringType.Name );
                return false;
            }

            return true;
        }

        public override void OnInvoke( MethodInterceptionArgs args )
        {
            DispatcherObject dispatcherObject = (DispatcherObject) args.Instance;

            if ( this.Async )
            {
                // Invoke the method asynchronously on the GUI thread.
                dispatcherObject.Dispatcher.BeginInvoke( this.priority, new Action( args.Proceed ) );
            }
            else if ( dispatcherObject.CheckAccess() )
            {
                // We have access to the GUI object. Invoke the method synchronously.
                args.Proceed();
            }
            else
            {
                // We don't have access to the GUI thread. Invoke the method synchronously on that thread.
                dispatcherObject.Dispatcher.Invoke( DispatcherPriority.Normal, new Action( args.Proceed ) );
            }
        }
    }
}