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
using System.Security;
using Librarian.BusinessProcesses;
using Librarian.Entities;
using Librarian.Framework;

namespace Librarian
{
    internal class ServerSession : MarshalByRefObject, ISession
    {
        private readonly Employee employee;
        private Cashbox cashbox;

        public ServerSession( Employee employee )
        {
            this.employee = employee;
        }

        public bool IsAuthenticated { get { return this.employee != null; } }

        public Employee Employee { get { return this.employee; } }

        public Cashbox Cashbox { get { return this.cashbox; } }

        #region ISession Members

        [Trace]
        public void SetCurrentCashbox( string cashBoxId )
        {
            CashboxProcesses cashboxProcesses = new CashboxProcesses( null );
            Cashbox cashbox = cashboxProcesses.FindCashBoxById( cashBoxId );

            if ( cashbox == null )
                throw new ArgumentOutOfRangeException( "cashBoxId" );

            this.cashbox = cashbox;
        }

        [Trace]
        public object GetService( string serviceName )
        {
            if ( !this.IsAuthenticated )
            {
                throw new SecurityException( "You are not authentified." );
            }

            switch ( serviceName )
            {
                case "EntityResolver":
                    return new EntityResolver( this );

                case "BookProcesses":
                    return new BookProcesses( this );

                case "CashboxProcesses":
                    return new CashboxProcesses( this );

                case "CustomerProcesses":
                    return new CustomerProcesses( this );

                case "EmployeeProcesses":
                    return new EmployeeProcesses( this );

                case "RentalProcesses":
                    return new RentalProcesses( this );

                case "NoteProcesses":
                    return new NoteProcesses( this );

                default:
                    throw new ArgumentOutOfRangeException( "serviceName" );
            }
        }

        #endregion
    }
}
