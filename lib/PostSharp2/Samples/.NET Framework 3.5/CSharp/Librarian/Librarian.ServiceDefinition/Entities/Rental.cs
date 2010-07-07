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
    public sealed class Rental : Entity
    {
        [Required] public EntityRef<Book> Book;

        [Required] public EntityRef<Customer> Customer;

        public DateTime StartDate;

        public DateTime ScheduledReturnDate;

        public DateTime? ReturnDate;

        public bool Closed;

        public bool IsDelayed()
        {
            return !this.Closed && this.ScheduledReturnDate < DateTime.Now;
        }
    }
}
