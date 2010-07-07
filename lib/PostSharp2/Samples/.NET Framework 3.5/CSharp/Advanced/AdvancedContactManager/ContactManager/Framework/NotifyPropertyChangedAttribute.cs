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
    [AspectTypeDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, typeof(ThrowWhenDisposedAttribute) )]
    [ProvideAspectRole( StandardRoles.DataBinding )]
    public class NotifyPropertyChangedAttribute : InstanceLevelAspect
    {
        [ImportMember( "OnPropertyChanged", IsRequired = true )] 
        public Action<string> OnPropertyChanged;

        private IEnumerable<PropertyInfo> SelectProperties( Type type )
        {
            return from property in type.GetProperties( BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public )
                   where !property.IsDefined( typeof(NoChangeNotificationAttribute), false ) &&
                         property.CanWrite
                   select property;
        }

        [OnLocationSetValueAdvice, MethodPointcut( "SelectProperties" )]
        public void OnSetValue( LocationInterceptionArgs args )
        {
            if ( args.Value != args.GetCurrentValue() )
            {
                args.ProceedSetValue();
                this.OnPropertyChanged( args.Location.Name );
            }
        }
    }
}