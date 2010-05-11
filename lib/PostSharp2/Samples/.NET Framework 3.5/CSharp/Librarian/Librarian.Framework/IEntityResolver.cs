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
    /// Interface of a service that retrieves an entity given its key.
    /// </summary>
    public interface IEntityResolver
    {
        /// <summary>
        /// Gets an entity given its key.
        /// </summary>
        /// <param name="entityKey">The entity key.</param>
        /// <returns>The <see cref="Entity"/> whose key is <paramref name="entityKey"/>, or <b>null</b>
        /// if no entity with this key exists.</returns>
        BaseEntity GetEntity( EntityKey entityKey );
    }
}