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

namespace DbInvoke
{
    internal class Program
    {
        private static void Assert( bool condition, string message )
        {
            if ( !condition )
                throw new ApplicationException( "Assertion failed: " + message );
        }

        private static void Main(  )
        {
            int customerId;
            string customerName;
            DataLayer.CreateCustomer( "Jack Pabon", out customerId );
            Assert( customerId != 0, "customerId != 0" );
            DataLayer.ModifyCustomer( customerId, "Joe Eifel" );
            DataLayer.ReadCustomer( customerId, out customerName );
            Assert( customerName == "Joe Eifel", "customerName == \"Joe Eifel\"" );
            DataLayer.DeleteCustomer( customerId );
        }
    }
}