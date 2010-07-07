using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Dependencies;
using PostSharp.Aspects.Serialization;
using PostSharp.Extensibility;

namespace ContactManager.Framework
{
    [MulticastAttributeUsage( MulticastTargets.Class, Inheritance = MulticastInheritance.Strict )]
    [AspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
    [AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Threading )]
    public sealed class ThrowWhenDisposedAttribute : InstanceLevelAspect
    {
        private object parentObject;

        public override object CreateInstance( AdviceArgs aspectArgs )
        {
            return new ThrowWhenDisposedAttribute {parentObject = aspectArgs.Instance};
        }

        [ImportMember( "IsDisposed", IsRequired = true )] public Property<bool> IsDisposed;


        private IEnumerable<MethodInfo> SelectLockedMethods( Type type )
        {
            const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            return from method
                       in type.GetMethods( bindingFlags )
                   where !method.IsDefined( typeof(SafeWhenDisposedAttribute), false )
                   select method;
        }

        [OnMethodEntryAdvice, MethodPointcut( "SelectLockedMethods" )]
        public void OnEntry( MethodExecutionArgs args )
        {
            if ( this.IsDisposed.Get() )
                throw new ObjectDisposedException( parentObject.ToString() );
        }
    }
}