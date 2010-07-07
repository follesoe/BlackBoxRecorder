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
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using PostSharp.Reflection.GenericArgs;

namespace DynamicComposition
{
    /// <summary>
    /// Simple implementation of <see cref="CompositionAspect"/> where the implementation
    /// and interface types are specified using the properties of this custom attribute.
    /// </summary>
    /// <remarks>
    /// This sample implementation does not support generics, because it is not possible
    /// to specify generic parameters in custom attributes.
    /// </remarks>
    [Serializable]
    public sealed class SimpleCompositionAttribute : CompositionAspect
    {
        #region Fields

        // Note that we have to store strings and not types, because
        // types are not serializable.

        // Name of the type of the composed object.
        private readonly string implementationTypeName;

        // Name of the exposed interface.
        private readonly string interfaceTypeName;

        #endregion

        public SimpleCompositionAttribute( Type interfaceType, Type implementationType )
        {
            if ( implementationType != null )
            {
                this.implementationTypeName = implementationType.AssemblyQualifiedName;
            }

            if ( interfaceType != null )
            {
                this.interfaceTypeName = interfaceType.AssemblyQualifiedName;
            }
        }

        #region Properties

        /// <summary>
        /// Gets or sets the type name of the composed object. Note that this
        /// type should implement the type of the interface (<see cref="InterfaceType"/>).
        /// </summary>
        public Type ImplementationType { get { return Type.GetType( this.implementationTypeName, true, false ); } }

        /// <summary>
        /// Gets or sets the name of the exposed interface.
        /// </summary>
        public Type InterfaceType { get { return Type.GetType( this.interfaceTypeName, true, false ); } }

        #endregion

        public override bool CompileTimeValidate( Type type )
        {
            if ( !base.CompileTimeValidate( type ) )
                return false;

            // Verify that mandatory arguments are given.
            if ( this.interfaceTypeName == null )
            {
                CompositionMessageSource.Instance.Write(
                    SeverityType.Error, "AG0001", new object[] {type.FullName, "interfaceType"} );
                return false;
            }

            if ( this.interfaceTypeName == null )
            {
                CompositionMessageSource.Instance.Write(
                    SeverityType.Error, "AG0001", new object[] {type.FullName, "implementationTypeName"} );
                return false;
            }


            return true;
        }

        #region Implementation

        /// <summary>
        /// Creates the composed object.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        /// <returns>A new instance of the type specified in the <see cref="ImplementationType"/> property.</returns>
        public override object CreateImplementationObject( AdviceArgs eventArgs )
        {
            return
                Activator.CreateInstance(
                    GenericArg.Map( this.ImplementationType, eventArgs.Instance.GetType().GetGenericArguments(), null ) );
        }

        /// <summary>
        /// Returns the interface to be exposed.
        /// </summary>
        /// <param name="containerType">Type of the object in which our instance will be composed.
        /// This is not relevant for our purpose.</param>
        /// <returns>The content of the <see cref="InterfaceType"/> property.</returns>
        protected override Type[] GetPublicInterfaces( Type containerType )
        {
            return new [] { GenericArg.Map( this.InterfaceType, containerType.GetGenericArguments(), null ) };
        }

        #endregion
    }
}