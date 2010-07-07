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

namespace DbInvoke
{

    /// <summary>
    /// Imports the semantics of the database (i.e. its stored procedures).
    /// </summary>
    [DbInvoke("ConnectionString")]
    internal static class DataLayer
    {
#pragma warning disable 626
        extern static public void CreateCustomer(string customerName, out int customerId);
        extern static public void ModifyCustomer(int customerId, string customerName);
        extern static public void DeleteCustomer(int customerId);
        extern static public void ReadCustomer(int customerId, out string customerName);
#pragma warning restore 626
    }
}
