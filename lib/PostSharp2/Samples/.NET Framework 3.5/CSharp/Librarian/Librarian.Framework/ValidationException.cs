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
using System.Runtime.Serialization;

namespace Librarian.Framework
{
    /// <summary>
    /// Exception thrown when a field value is invalid with respect to
    /// a field validator (<see cref="FieldValidationAttribute"/>).
    /// </summary>
    [Serializable]
    public class ValidationException : BusinessException
    {
        /// <summary>
        /// Initializes a new <see cref="ValidationException"/>.
        /// </summary>
        /// <param name="fieldName">Name of the invalid field.</param>
        /// <param name="fieldValue">Field value.</param>
        /// <param name="message">Message.</param>
        public ValidationException( string fieldName, object fieldValue, string message )
            : base( ComposeMessage( fieldName, fieldValue, message ) )
        {
        }

        protected ValidationException(
            SerializationInfo info,
            StreamingContext context )
            : base( info, context )
        {
        }


        private static string ComposeMessage( string fieldName, object fieldValue, string message )
        {
            return string.Format( "Invalid value {{{0}}} for field {{{1}}}: {2}", fieldValue, fieldName, message );
        }
    }
}