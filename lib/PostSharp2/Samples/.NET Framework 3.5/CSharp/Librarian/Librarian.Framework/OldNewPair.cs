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

namespace Librarian.Framework
{
    /// <summary>
    /// Pair of two properties of the same type, <see cref="OldValue"/> and <see cref="NewValue"/>.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public sealed class OldNewPair<T>
    {
        private readonly T oldValue;
        private readonly T newValue;

        /// <summary>
        /// Initializes a new <see cref="OldNewPair{T}"/>.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        public OldNewPair( T oldValue, T newValue )
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        /// <summary>
        /// Gets the old value.
        /// </summary>
        public T OldValue { get { return this.oldValue; } }

        /// <summary>
        /// Gets the new value.
        /// </summary>
        public T NewValue { get { return this.newValue; } }
    }
}