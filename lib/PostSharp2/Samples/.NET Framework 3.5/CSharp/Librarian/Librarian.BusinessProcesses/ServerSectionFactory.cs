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
using System.Security.Authentication;
using Librarian.BusinessProcesses;
using Librarian.Entities;
using Librarian.Framework;

namespace Librarian
{
    internal class ServerSectionFactory : MarshalByRefObject, ISessionFactory
    {
        private readonly EmployeeProcesses employeeProcesses = new EmployeeProcesses( null );

        [Trace]
        public ISession OpenSession( string login, string password )
        {
            // Authenticate.
            Employee employee = this.employeeProcesses.FindEmployeeByLogin( login );
            if ( employee == null || !employee.Authenticate( password ) )
            {
                throw new AuthenticationException("Invalid login or password.");
            }
            else
            {
                return new ServerSession( employee );
            }
        }
    }
}