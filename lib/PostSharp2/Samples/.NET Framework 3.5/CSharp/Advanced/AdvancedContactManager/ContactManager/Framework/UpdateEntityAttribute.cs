using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Dependencies;
using PostSharp.Aspects.Serialization;
using PostSharp.Extensibility;
using Threading;

namespace ContactManager.Framework
{
    [AspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
    [MulticastAttributeUsage( MulticastTargets.Class, Inheritance = MulticastInheritance.Strict )]
    [AspectTypeDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, typeof(ThrowWhenDisposedAttribute) )]
    [AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.DataBinding )]
    public sealed class UpdateEntityAttribute : InstanceLevelAspect
    {
        private Entity parent;

        public override object CreateInstance(AdviceArgs aspectArgs)
        {
            return new UpdateEntityAttribute {parent = (Entity) aspectArgs.Instance};
        }

        [ImportMember( "OnEntityPropertyUpdated", IsRequired = true )] 
        public Action OnEntityPropertyUpdated;

        private IEnumerable<PropertyInfo> SelectEntityProperties( Type type )
        {
            return from property
                       in type.GetProperties( BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic )
                   where property.CanWrite
                   select property;
        }

        [OnLocationSetValueAdvice, MethodPointcut( "SelectEntityProperties" )]
        public void SetProperty( LocationInterceptionArgs args )
        {
            if ( args.Value != args.GetCurrentValue() )
            {
                using ( Post.Cast<Entity, IReaderWriterSynchronized>( parent ).AcquireWriteLock() )
                {
                    // Change the EntityStatus flag before the value
                    // is actually changed, so the PropertyChanged
                    // event is fired when EntityStatus is correct.
                    this.OnEntityPropertyUpdated();
                    args.ProceedSetValue();
                }
            }
        }
    }
}