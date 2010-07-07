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
using PostSharp.Sdk.AspectWeaver;
using PostSharp.Sdk.Extensibility;

namespace Librarian.Weaver
{
    /// <summary>
    /// Creates the weavers defined by the 'PostSharp.Samples.Librarian' plug-in.
    /// </summary>
    public class LibrarianPlugIn : AspectWeaverPlugIn
    {
        public LibrarianPlugIn( ) : base( StandardPriorities.User )
        {
        }

        protected override void Initialize()
        {
            this.BindAspectWeaver<EntityAspect, ImplementCloneableAspectWeaver>();
            this.BindAspectWeaver<TypeValidationAspect, ImplementValidableAspectWeaver>();
        }
        
    }
}