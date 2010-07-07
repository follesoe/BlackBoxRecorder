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
    /// Encapsulation of an object with a name, typically used when the output
    /// of the <b>ToString</b> method is not human-readable, but a consumer
    /// requires a human-readable output (like combobox items).
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public sealed class NameValuePair<T>
    {
        private readonly T value;
        private readonly string name;

        /// <summary>
        /// Initializes a new <see cref="NameValuePair{T}"/>.
        /// </summary>
        /// <param name="value">Value (exposed in the <see cref="Value"/> property).</param>
        /// <param name="name">Name (returned by the <see cref="ToString"/> method).</param>
        public NameValuePair( T value, string name )
        {
            this.value = value;
            this.name = name;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value { get { return this.value; } }

        /// <summary>
        /// Gets a human-readable description (the name) of this object.
        /// </summary>
        /// <returns>A human-readable description (the name) of this object.</returns>
        public override string ToString()
        {
            return this.name;
        }
    }
}