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
    public sealed class Customer : Entity
    {
        [Required] public string CustomerId;

        [Required] public string FirstName;

        [Required] public string LastName;

        public bool Deleted;

        public int MaxOpenRentals = 5;

        public decimal CurrentBalance;
    }
}
