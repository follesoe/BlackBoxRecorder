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
using System.Reflection;
using System.Text;

namespace Librarian.Framework
{
    /// <summary>
    /// Provides formatting string representing types, methods and fields. The
    /// formatting strings may contain arguments like <c>{0}</c> 
    /// filled at runtime with generic parameters or method arguments.
    /// </summary>
    internal static class TraceFormatter
    {
        /// <summary>
        /// Gets a formatting string representing a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/>.</param>
        /// <returns>A formatting string representing the type
        /// where each generic type argument is represented as a
        /// formatting argument (e.g. <c>Dictionary&lt;{0},P1}&gt;</c>.
        /// </returns>
        public static string GetTypeFormatString( Type type )
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Build the format string for the declaring type.

            stringBuilder.Append( type.FullName );

            if ( type.IsGenericTypeDefinition )
            {
                stringBuilder.Append( "<" );
                for ( int i = 0 ; i < type.GetGenericArguments().Length ; i++ )
                {
                    if ( i > 0 )
                        stringBuilder.Append( ", " );
                    stringBuilder.AppendFormat( "{{{0}}}", i );
                }
                stringBuilder.Append( ">" );
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the formatting strings representing a method.
        /// </summary>
        /// <param name="method">A <see cref="MethodBase"/>.</param>
        /// <returns></returns>
        public static MethodFormatStrings GetMethodFormatStrings( MethodBase method )
        {
            string typeFormat;
            bool typeIsGeneric;
            string methodFormat;
            bool methodIsGeneric;
            string parameterFormat;

            StringBuilder stringBuilder = new StringBuilder();

            typeFormat = GetTypeFormatString( method.DeclaringType );
            typeIsGeneric = method.DeclaringType.IsGenericTypeDefinition;

            // Build the format string for the method name.
            stringBuilder.Length = 0;
            stringBuilder.Append( "::" );
            stringBuilder.Append( method.Name );
            if ( method.IsGenericMethodDefinition )
            {
                methodIsGeneric = true;
                stringBuilder.Append( "<" );
                for ( int i = 0 ; i < method.GetGenericArguments().Length ; i++ )
                {
                    if ( i > 0 )
                        stringBuilder.Append( ", " );
                    stringBuilder.AppendFormat( "{{{0}}}", i );
                }
                stringBuilder.Append( ">" );
            }
            else
            {
                methodIsGeneric = false;
            }
            methodFormat = stringBuilder.ToString();

            // Build the format string for parameters.
            stringBuilder.Length = 0;
            ParameterInfo[] parameters = method.GetParameters();
            stringBuilder.Append( "(" );
            for ( int i = 0 ; i < parameters.Length ; i++ )
            {
                if ( i > 0 )
                {
                    stringBuilder.Append( ", " );
                }
                stringBuilder.Append( "{{{" );
                stringBuilder.Append( i );
                stringBuilder.Append( "}}}" );
            }
            stringBuilder.Append( ")" );

            parameterFormat = stringBuilder.ToString();

            return new MethodFormatStrings( typeFormat, typeIsGeneric, methodFormat, methodIsGeneric, parameterFormat );
        }

        /// <summary>
        /// Pads a string with a space, if not empty and not yet padded.
        /// </summary>
        /// <param name="prefix">A string.</param>
        /// <returns>A padded string.</returns>
        public static string NormalizePrefix( string prefix )
        {
            if ( string.IsNullOrEmpty( prefix ) )
            {
                return "";
            }
            else if ( prefix.EndsWith( " " ) )
            {
                return prefix;
            }
            else
            {
                return prefix + " ";
            }
        }

        public static string FormatString( string format, params object[] args )
        {
            if ( args == null )
                return format;
            else
                return string.Format( format, args );
        }
    }

    /// <summary>
    /// Set of 3 formatting string that, at runtime, represent a method and its
    /// parameters.
    /// </summary>
    [Serializable]
    internal class MethodFormatStrings
    {
        private readonly string typeFormat;
        private readonly string methodFormat;
        private readonly string parameterFormat;
        private readonly bool typeIsGeneric;
        private readonly bool methodIsGeneric;

        /// <summary>
        /// Initializes a new <see cref="MethodFormatStrings"/>.
        /// </summary>
        /// <param name="typeFormat">
        /// The formatting string representing the type
        /// where each generic type argument is represented as a
        /// formatting argument (e.g. <c>Dictionary&lt;{0},P1}&gt;</c>.
        /// </param>
        /// <param name="methodFormat">
        /// The formatting string representing the method (but not the declaring type).
        /// where each generic method argument is represented as a
        /// formatting argument (e.g. <c>ToArray&lt;{0}&gt;</c>. 
        /// </param>
        /// <param name="parameterFormat">
        /// The formatting string representing the list of parameters, where each
        /// parameter is representing as a formatting argument 
        /// (e.g. <c>{0}, {1}</c>).        
        /// </param>
        /// <param name="methodIsGeneric">Determines whether the method is generic.</param>
        /// <param name="typeIsGeneric">Determines whether the type declaring the method is generic.</param>
        internal MethodFormatStrings( string typeFormat, bool typeIsGeneric,
                                      string methodFormat,
                                      bool methodIsGeneric,
                                      string parameterFormat )
        {
            this.typeFormat = typeFormat;
            this.methodFormat = methodFormat;
            this.parameterFormat = parameterFormat;
            this.typeIsGeneric = typeIsGeneric;
            this.methodIsGeneric = methodIsGeneric;
        }


        /// <summary>
        /// Gets a string representing a concrete method invocation.
        /// </summary>
        /// <param name="instance">Instance on which the method is invoked, or <b>null</b> with a static method.</param>
        /// <param name="method">Invoked method.</param>
        /// <param name="invocationParameters">Concrete invocation parameters.</param>
        /// <returns>A representation of the method invocation.</returns>
        public string Format(
            object instance,
            MethodBase method,
            object[] invocationParameters )
        {
            string[] parts = new string[4]
                {
                    typeIsGeneric
                        ? TraceFormatter.FormatString( this.typeFormat, method.DeclaringType.GetGenericArguments() )
                        : this.typeFormat,
                    methodIsGeneric
                        ? TraceFormatter.FormatString( this.methodFormat, method.GetGenericArguments() )
                        : this.methodFormat,
                    instance == null ? "" : string.Format( "{{{0}}}", instance ),
                    TraceFormatter.FormatString( this.parameterFormat, invocationParameters )
                };

            return string.Concat( parts );
        }
    }
}