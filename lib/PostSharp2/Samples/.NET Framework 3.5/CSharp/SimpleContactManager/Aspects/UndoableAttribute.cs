using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

namespace ContactManager.Aspects
{
    [Serializable]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.DataBinding)]
    public sealed class UndoableAttribute : InstanceLevelAspect
    {
        [ImportMember( "OnPropertyChanged", IsRequired = true )] 
        public Action<string> OnPropertyChanged;

        [ImportMember( "IsInitialized", IsRequired = false )] 
        public Property<bool> IsInitialized;

        [OnLocationSetValueAdvice, MulticastPointcut( Targets = MulticastTargets.Field,
            Attributes =
                MulticastAttributes.Instance )]
        public void OnSetValue( LocationInterceptionArgs args )
        {
            object currentValue = args.GetCurrentValue();
            if ( args.Value == currentValue )
                return;

            if ( this.IsInitialized == null || this.IsInitialized.Get() )
            {
                // Record changes only when the object is initialized.
                UndoManager.Record( new UndoItem( this, args.Value, currentValue, args ) );
            }

            args.ProceedSetValue();
        }

        private class UndoItem : IUndoItem
        {
            private readonly UndoableAttribute parent;
            private readonly object newValue, oldValue;
            private readonly LocationInterceptionArgs args;

            public UndoItem( UndoableAttribute parent, object newValue, object oldValue, LocationInterceptionArgs args )
            {
                this.newValue = newValue;
                this.parent = parent;
                this.oldValue = oldValue;
                this.args = args;
            }

            public string Description
            {
                get { return string.Format( "Changing '{0}' -> '{1}'", newValue, oldValue ); }
            }

            public void Redo()
            {
                args.Value = newValue;
                args.ProceedSetValue();
                this.parent.OnPropertyChanged( null );
            }

            public void Undo()
            {
                args.Value = oldValue;
                args.ProceedSetValue();
                this.parent.OnPropertyChanged( null );
            }
        }
    }
}