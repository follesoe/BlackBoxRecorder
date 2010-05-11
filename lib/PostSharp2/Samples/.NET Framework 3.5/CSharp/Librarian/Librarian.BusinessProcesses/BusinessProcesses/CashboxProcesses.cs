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
using Librarian.Data;
using Librarian.Entities;
using Librarian.Framework;

namespace Librarian.BusinessProcesses
{
    [Trace]
    internal class CashboxProcesses : SessionBoundService, ICashboxProcesses
    {
        public CashboxProcesses( ServerSession session )
            : base( session )
        {
        }

        [Transaction]
        public void RegisterCashboxOperation( CashboxOperation operation )
        {
            if ( operation == null )
                throw new ArgumentNullException( "operation" );

            BusinessRulesManager.Assert( "RegisterCashboxOperation", operation );

            Cashbox cashbox = operation.Cashbox.GetVanillaEntity();
            cashbox.Balance += operation.Amount;

            StorageContext.Current.Insert( operation );
            StorageContext.Current.Update( cashbox );
        }

        public Cashbox FindCashBoxById( string cashBoxId )
        {
            foreach ( Cashbox cashBox in StorageContext.Current.Find<Cashbox>(
                delegate( Cashbox candidate ) { return string.Compare( candidate.CashboxId, cashBoxId, true ) == 0; } )
                )
            {
                return cashBox;
            }

            return null;
        }
    }
}