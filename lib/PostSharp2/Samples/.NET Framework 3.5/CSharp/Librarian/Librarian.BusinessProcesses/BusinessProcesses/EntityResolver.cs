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

using Librarian.Data;
using Librarian.Entities;
using Librarian.Framework;

namespace Librarian.BusinessProcesses
{
    [Trace]
    internal class EntityResolver : SessionBoundService, IEntityResolver
    {
        public EntityResolver( ServerSession session )
            : base( session )
        {
        }

        public BaseEntity GetEntity( EntityKey entityKey )
        {
            return StorageContext.Current.GetEntity( entityKey );
        }
    }
}