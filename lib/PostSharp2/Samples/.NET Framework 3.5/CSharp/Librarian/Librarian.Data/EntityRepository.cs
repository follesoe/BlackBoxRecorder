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
using System.Text;
using Librarian.Framework;

namespace Librarian.Data
{

    /// <summary>
    /// A dictionary of entities exposing discovery methods (<b>Find</b>, <b>Exists</b>).
    /// </summary>
    public abstract class EntityRepository
    {
        readonly Dictionary<EntityKey, BaseEntity> entities = new Dictionary<EntityKey, BaseEntity>();


        #region Dictionary update
        protected void InternalSet(BaseEntity BaseEntity)
        {
            this.entities[BaseEntity.EntityKey] = BaseEntity;
        }

        protected void InternalRemove(EntityKey EntityKey)
        {
            this.entities.Remove(EntityKey);
        }

        protected BaseEntity InternalGet(EntityKey EntityKey)
        {
            BaseEntity BaseEntity;
            this.entities.TryGetValue(EntityKey, out BaseEntity);
            return BaseEntity;
        }

        protected void InternalClear()
        {
            this.entities.Clear();
        }
        #endregion

        #region Discovery methods

        /// <summary>
        /// Gets all entities contained in this repository.
        /// </summary>
        /// <returns>An enumerator.</returns>
        /// <remarks>
        /// This method can be overridden if 'fall-back' mechanisms should be used 
        /// (when not in this repository, look elsewhere).
        /// </remarks>
        protected virtual IEnumerator<BaseEntity> GetAllEntitiesEnumerator()
        {
            return this.entities.Values.GetEnumerator();
        }


        internal IEnumerator<T> InternalFind<T>(Predicate<BaseEntity> predicate, int max)
           where T : BaseEntity
        {
            IEnumerator<BaseEntity> enumerator = this.GetAllEntitiesEnumerator();
            int i = 0;
            while (enumerator.MoveNext() && (max < 0 || i < max))
            {
                BaseEntity BaseEntity = enumerator.Current;

                if (predicate(BaseEntity))
                {
                    i++;
                    yield return (T)BaseEntity.Clone();
                }
            }
        }

        public IEnumerable<BaseEntity> Find(Predicate<BaseEntity> predicate)
        {
            return this.Find(predicate, -1);
        }

        public IEnumerable<BaseEntity> Find(Predicate<BaseEntity> predicate, int max)
        {
            return new Enumerable<BaseEntity>( InternalFind<BaseEntity>(predicate, max));
        }

        public IEnumerable<T> Find<T>(Predicate<T> predicate)
            where T : BaseEntity
        {
            return this.Find<T>(predicate, -1);
        }

        public IEnumerable<T> Find<T>(Predicate<T> predicate, int max)
            where T : BaseEntity
        {
            return new List<T>(new Enumerable<T>(InternalFind<T>(
               
                delegate(BaseEntity BaseEntity)
                {
                    T typedBaseEntity = BaseEntity as T;
                    return typedBaseEntity != null && ( predicate == null || predicate(typedBaseEntity) );
                }, max)));
        }

        public bool Exists<T>(Predicate<T> predicate)
            where T : BaseEntity
        {
            IEnumerator<BaseEntity> enumerator = this.GetAllEntitiesEnumerator();
            while (enumerator.MoveNext())
            {
                BaseEntity BaseEntity = enumerator.Current;
                T typedBaseEntity = BaseEntity as T;
                if (typedBaseEntity != null && (predicate == null || predicate(typedBaseEntity)))
                    return true;
            }

            return false;
        }
        #endregion
    }
}
