#region Released to Public Domain by Gael Fraiteur

/*----------------------------------------------------------------------------*
 *   This file is part of samples of PostSharp.                                *
 *                                                                             *
 *   This sample is free software: you have an unlimited right to              *
 *   redistribute it and/or modify it.                                         *
 *                                                                             *
 *   This sample is distributed in the hope that it will be useful,            *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of            *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                      *
 *                                                                             *
 *----------------------------------------------------------------------------*/

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

namespace ContactManager.Aspects
{
    /// <summary>
    /// Custom attribute that, when applied on a type (designated <i>target type</i>), implements the interface
    /// <see cref="INotifyPropertyChanged"/> and raises the <see cref="INotifyPropertyChanged.PropertyChanged"/>
    /// event when any property of the target type is modified.
    /// </summary>
    /// <remarks>
    /// Event raising is implemented by appending logic to the <b>set</b> accessor of properties. The 
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/> is raised only when accessors successfully complete.
    /// </remarks>
    [MulticastAttributeUsage( MulticastTargets.Class | MulticastTargets.Struct, Inheritance = MulticastInheritance.Strict )]
    [Serializable]
    [IntroduceInterface( typeof(INotifyPropertyChanged), OverrideAction = InterfaceOverrideAction.Ignore )]
    [ProvideAspectRole(StandardRoles.DataBinding)]
    public sealed class NotifyPropertyChangedAttribute : InstanceLevelAspect, INotifyPropertyChanged
    {
        [IntroduceMember(OverrideAction = MemberOverrideAction.Ignore)]
        public event PropertyChangedEventHandler PropertyChanged;

        [IntroduceMember( OverrideAction = MemberOverrideAction.Ignore )]
        public void OnPropertyChanged( string propertyName )
        {
            if ( this.PropertyChanged != null )
                this.PropertyChanged( this.Instance, new PropertyChangedEventArgs( propertyName ) );
        }

        [ImportMember("OnPropertyChanged", Order = ImportMemberOrder.AfterIntroductions)]
        public Action<string> OnPropertyChangedMethod;

        private IEnumerable<PropertyInfo> SelectProperties( Type type )
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | 
                BindingFlags.DeclaredOnly
                                              | BindingFlags.Public;

            return from property
                       in type.GetProperties( bindingFlags )
                   where property.CanWrite
                   select property;
        }

        [OnLocationSetValueAdvice, MethodPointcut( "SelectProperties" )]
        public void OnSetValue( LocationInterceptionArgs args )
        {
            if ( args.Value != args.GetCurrentValue() )
            {
                args.ProceedSetValue();

               this.OnPropertyChangedMethod.Invoke(null);
            }
        }
    }
}