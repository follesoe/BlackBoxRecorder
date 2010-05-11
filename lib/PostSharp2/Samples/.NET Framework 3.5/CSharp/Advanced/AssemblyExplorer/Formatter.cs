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

#region Using directives

using System;
using System.Text;

#endregion

namespace AssemblyExplorer
{
    public static class Formatter
    {
        public static void Format( object value, StringBuilder builder )
        {
            if ( value == null )
            {
                builder.Append( "<null>" );
            }
            else if ( value is string )
            {
                builder.Append( '"' );
                builder.Append( (string) value );
                builder.Append( '"' );
            }
            else if ( value is Array )
            {
                Array array = (Array) value;
                builder.Append( " {" );
                for ( int i = 0 ; i < array.Length ; i++ )
                {
                    if ( i > 0 )
                    {
                        builder.Append( ", " );
                    }
                    Format( array.GetValue( i ), builder );
                }
                builder.Append( " }" );
            }
            else
            {
                builder.Append( value );
            }
        }
    }
}