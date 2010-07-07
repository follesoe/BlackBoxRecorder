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

namespace Librarian.Framework
{
    /// <summary>
    /// Collection of entities (or of entity references, to be precised)
    /// to be used in entity members.
    /// </summary>
    /// <remarks>
    /// This collection enforces that valid references (not null) are given.
    /// </remarks>
    /// <typeparam name="T">Type of entity references stored in this collection.</typeparam>
    [Serializable]
    public class EntityCollection<T> : CloningCollection<EntityRef<T>>
        where T : BaseEntity, new()
    {
        /// <summary>
        /// Initializes an <see cref="EntityCollection"/> with a given initial capacity.
        /// </summary>
        /// <param name="capacity">Initial capacity.</param>
        public EntityCollection( int capacity )
            : base( capacity )
        {
        }

        /// <summary>
        /// Initializes a new <see cref="EntityCollection"/>.
        /// </summary>
        public EntityCollection() : this( 4 )
        {
        }

        protected override void InsertItem( int index, EntityRef<T> item )
        {
            if ( item.IsNull )
                throw new ArgumentNullException( "item" );
            base.InsertItem( index, item );
        }

        protected override void SetItem( int index, EntityRef<T> item )
        {
            if ( item.IsNull )
                throw new ArgumentNullException( "item" );
            base.SetItem( index, item );
        }

        protected override CloningCollection<EntityRef<T>> CreateInstance( int capacity )
        {
            return new EntityCollection<T>( capacity );
        }


        /// <summary>
        /// Determines whether the current collection contains an entity
        /// that fulfills a given predicates.
        /// </summary>
        /// <param name="predicate">A predicate.</param>
        /// <returns></returns>
        public bool Exists( Predicate<T> predicate )
        {
            if ( predicate == null )
                throw new ArgumentNullException( "predicate" );

            foreach ( EntityRef<T> item in this )
            {
                if ( predicate( item.Entity ) )
                {
                    return true;
                }
            }

            return false;
        }
    }
}