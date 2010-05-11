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
    /// <summary>
    /// Base for all entities.
    /// </summary>
    /// <remarks>
    /// An entity is the root of an object tree (not graph!) that can
    /// be stored atomically in database.
    /// </remarks>
    /// <note>Modifying an entity member does not directly modify the
    /// entity in database. All changes should go through the
    /// process layer.</note>
    [Serializable]
    [EntityAspect]
    public abstract class Entity : BaseEntity
    {
      
        /// <summary>
        /// Initializes a new <see cref="Entity"/>.
        /// </summary>
        protected Entity()
        {
        }

   /// <summary>
        /// Clones the current object.
        /// </summary>
        /// <returns>A deep clone of the current object.</returns>
        public new Entity Clone()
        {
            return (Entity) base.Clone();
        }


    }
}