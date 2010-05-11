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
    public class InitializationProcesses
    {
        [Transaction]
        public void CheckDatabaseInitialized()
        {
            if ( !StorageContext.Current.Exists<Employee>( null ) )
            {
                Employee employee = new Employee {FirstName = "Initial", LastName = "Employee", Login = "init"};
                employee.SetPassword( "init" );
                StorageContext.Current.Insert( employee );
            }

            if ( !StorageContext.Current.Exists<Cashbox>( null ) )
            {
                Cashbox cashbox = new Cashbox {CashboxId = "1", Name = "My Cashbox", Balance = 0};
                StorageContext.Current.Insert( cashbox );
            }
        }
    }
}