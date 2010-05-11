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

using System.Resources;
using PostSharp.Extensibility;

namespace Librarian.Framework
{
    /// <summary>
    /// Source of errors and warnings written to MSBuild output.
    /// </summary>
    internal static class LibrarianMessageSource
    {
        public static readonly MessageSource Instance = new MessageSource(
            "PostSharp.Samples.Librarian.Framework",
            new ResourceManager( "PostSharp.Samples.Librarian.Framework.ValidationErrors",
                                 typeof(LibrarianMessageSource).Assembly ) );
    }
}