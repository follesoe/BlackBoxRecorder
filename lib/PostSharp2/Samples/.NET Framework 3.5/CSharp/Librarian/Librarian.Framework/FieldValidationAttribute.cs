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
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;

namespace Librarian.Framework
{
    /// <summary>
    /// Base class for field validation custom attributes.
    /// </summary>
    [Serializable]
    [RequirePostSharp("Librarian", "Librarian")]
    public abstract class FieldValidationAttribute : LocationInterceptionAspect, IAspectProvider
    {
        private string fieldName;

        [NonSerialized]
        private LocationInfo targetLocation;

        internal LocationInfo TargetLocation { get { return this.targetLocation; } }
        protected string FieldName { get { return this.fieldName; } }

        /// <summary>
        /// Called at compile-time to initialize the current instance.
        /// </summary>
        /// <param name="field">Field to which the current custom attribute is applied.</param>
        public override void CompileTimeInitialize( LocationInfo location, AspectInfo aspectInfo )
        {
            this.targetLocation = location;
            this.fieldName = location.DeclaringType.Name + "." + location.Name;
        }


        /// <summary>
        /// Called at runtime whenever one modifies the field to which the current custom attribute is applied.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        public override void OnSetValue( LocationInterceptionArgs args )
        {
            this.ValidateFieldValue( args.Value );
            base.OnSetValue( args );
        }


        /// <summary>
        /// When implemented by a derived class, validates a value.
        /// </summary>
        /// <param name="value">The field value.</param>
        public abstract void ValidateFieldValue( object value );


        public IEnumerable<AspectInstance> ProvideAspects( object targetElement )
        {
            return TypeValidationAspect.RegisterFieldValidator( (LocationInfo) targetElement, this );
        }
    }
}