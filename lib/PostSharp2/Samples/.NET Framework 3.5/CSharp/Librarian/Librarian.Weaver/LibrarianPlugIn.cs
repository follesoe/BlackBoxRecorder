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

using Librarian.Framework;
using PostSharp.AspectWeaver;
using PostSharp.Extensibility;

namespace Librarian.Weaver
{
    /// <summary>
    /// Creates the weavers defined by the 'PostSharp.Samples.Librarian' plug-in.
    /// </summary>
    public class LibrarianPlugIn : PlugIn
    {
        public LibrarianPlugIn( ) : base( Priorities.User )
        {
        }

        protected override void Initialize()
        {
            this.AddAspectWeaverFactory<EntityAspect, ImplementCloneableAspectWeaver>();
            this.AddAspectWeaverFactory<TypeValidationAspect, ImplementValidableAspectWeaver>();
        }
        
    }
}