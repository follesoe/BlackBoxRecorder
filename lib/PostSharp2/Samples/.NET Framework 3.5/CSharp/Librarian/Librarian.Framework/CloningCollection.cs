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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Librarian.Framework
{
    /// <summary>
    /// Collection providing deep cloning.
    /// </summary>
    /// <typeparam name="T">Type of elements.</typeparam>
    [Serializable]
    public abstract class CloningCollection<T> : Collection<T>, ICloneable
        where T : ICloneable
    {
        /// <summary>
        /// Initializes a new <see cref="CloningCollection{T}"/> and determines the initial capacity.
        /// </summary>
        /// <param name="capacity">Initial capacity.</param>
        public CloningCollection( int capacity )
            : base( new List<T>( capacity ) )
        {
        }

        /// <summary>
        /// Initializes a new <see cref="CloningCollection{T}"/> with default capacity.
        /// </summary>
        public CloningCollection()
            : this( 4 )
        {
        }

        /// <summary>
        /// Clones the current collection.
        /// </summary>
        /// <returns>A deep clone of the current collection.</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Clones the current collection.
        /// </summary>
        /// <returns>A deep clone of the current collection.</returns>
        public CloningCollection<T> Clone()
        {
            CloningCollection<T> clone = this.CreateInstance( this.Count );
            this.CopyTo( clone );
            return clone;
        }

        /// <summary>
        /// Copies the content of the current collection into
        /// another one. Items are cloned before being copied
        /// in the target collection.
        /// </summary>
        /// <param name="collection">Target collection.</param>
        public virtual void CopyTo( ICollection<T> collection )
        {
            if ( collection == null )
                throw new ArgumentNullException( "collection" );

            foreach ( T item in this.Items )
            {
                collection.Add( (T) item.Clone() );
            }
        }

        /// <summary>
        /// When implemented in a derived class, creates a new and empty instance
        /// of the current collection with a given capacity.
        /// </summary>
        /// <param name="capacity">Initial capacity of the new collection.</param>
        /// <returns>A new collection (of same type as the current type) with
        /// the given initial capacity.</returns>
        protected abstract CloningCollection<T> CreateInstance( int capacity );
    }
}