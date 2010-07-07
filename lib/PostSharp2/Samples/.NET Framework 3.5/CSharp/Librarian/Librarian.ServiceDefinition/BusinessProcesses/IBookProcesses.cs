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

using System.Collections.Generic;
using Librarian.Entities;

namespace Librarian.BusinessProcesses
{
    public interface IBookProcesses : IStatefulService
    {
        Book CreateBook( Book book );

        IEnumerable<Book> FindBooks( string bookId, string authors, string title, string isbn, int max );
    }
}