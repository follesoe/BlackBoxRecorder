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

using System.Collections;
using System.Collections.Generic;

namespace Librarian.Framework
{
    /// <summary>
    /// Converts an <see cref="IEnumerator{T}"/> into an <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    public class Enumerable<T> : IEnumerable<T>
    {
        private IEnumerator<T> enumerator;

        public Enumerable( IEnumerator<T> enumerator )
        {
            this.enumerator = enumerator;
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return this.enumerator;
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}