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

namespace Librarian.Entities
{
    [Serializable]
    public sealed class CashboxOperation : Entity
    {
        [Required] public EntityRef<Cashbox> Cashbox;

        [Required] public EntityRef<Employee> Employee;

        public DateTime Date;

        [Required] public string Description;

        public EntityRef<CustomerAccountLine> CustomerAccountLine;

        public decimal Amount;
    }
}