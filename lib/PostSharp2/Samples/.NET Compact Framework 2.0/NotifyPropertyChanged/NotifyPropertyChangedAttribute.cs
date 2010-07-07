using System;
using System.ComponentModel;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Extensibility;
using PostSharp.Reflection;

namespace NotifyPropertyChanged
{
    [Serializable]
    [IntroduceInterface( typeof(INotifyPropertyChanged), OverrideAction = InterfaceOverrideAction.Ignore )]
    [MulticastAttributeUsage( MulticastTargets.Class, Inheritance = MulticastInheritance.Strict )]
    public sealed class NotifyPropertyChangedAttribute : InstanceLevelAspect, INotifyPropertyChanged
    {
       
        [ImportMember( "OnPropertyChanged", IsRequired = false, Order = ImportMemberOrder.AfterIntroductions)] 
        public Action<string> OnPropertyChangedMethod;

        [IntroduceMember( Visibility = Visibility.Family, IsVirtual = true, OverrideAction = MemberOverrideAction.Ignore )]
        public void OnPropertyChanged( string propertyName )
        {
            if ( this.PropertyChanged != null )
            {
                this.PropertyChanged( this.Instance, new PropertyChangedEventArgs( propertyName ) );
            }
        }

        [IntroduceMember( OverrideAction = MemberOverrideAction.Ignore )]
        public event PropertyChangedEventHandler PropertyChanged;

        [OnLocationSetValueAdvice, MulticastPointcut( Targets = MulticastTargets.Property, Attributes = MulticastAttributes.Instance | MulticastAttributes.NonAbstract)]
        public void OnPropertySet( LocationInterceptionArgs args )
        {
            // Don't go further if the new value is equal to the old one.
            // (Possibly use object.Equals here).
            if ( args.Value == args.GetCurrentValue() ) return;

            // Actually sets the value.
            args.ProceedSetValue();

            this.OnPropertyChangedMethod.Invoke( args.Location.Name );
            
        }
    }
}