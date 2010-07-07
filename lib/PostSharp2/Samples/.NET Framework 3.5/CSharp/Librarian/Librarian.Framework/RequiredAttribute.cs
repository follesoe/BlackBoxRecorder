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
using System.Data.SqlTypes;

namespace Librarian.Framework
{
    /// <summary>
    /// Custom attribute that, when applied on a field, specifies that it is required
    /// and cannot be null.
    /// </summary>
    [Serializable]
    public class RequiredAttribute : FieldValidationAttribute
    {
        private bool allowEmptyString;

        /// <summary>
        /// Whether empty strings should be allowed (they are not by default).
        /// </summary>
        /// <remarks>
        /// This property is irrelevant when the custom attribute is applied to
        /// a non-string field.
        /// </remarks>
        public bool AllowEmptyString { get { return allowEmptyString; } set { allowEmptyString = value; } }

        /// <summary>
        /// Validates the field value.
        /// </summary>
        /// <param name="value">Field value.</param>
        public override void ValidateFieldValue( object value )
        {
            if ( value == null ||
                 ( value is string && (string) value == "" && !this.allowEmptyString ) ||
                 ( value is INullable ) && ( (INullable) value ).IsNull )
            {
                throw new ValidationException( this.FieldName, value, "This field is required." );
            }
        }
    }
}