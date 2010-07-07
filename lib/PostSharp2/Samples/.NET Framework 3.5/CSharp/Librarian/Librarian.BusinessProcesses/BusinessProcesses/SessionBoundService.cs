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
using Librarian.Framework;

namespace Librarian.BusinessProcesses
{
    [Trace]
    internal abstract class SessionBoundService : MarshalByRefObject, IStatefulService
    {
        private readonly ServerSession session;

        protected SessionBoundService( ServerSession session )
        {
            this.session = session;
        }

        public ServerSession Session { get { return this.session; } }

        ISession IStatefulService.Session { get { return this.session; } }
    }
}