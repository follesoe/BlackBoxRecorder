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
using System.Data;
using System.Threading;
using System.Transactions;
using Librarian.Framework;

namespace Librarian.Data
{
    /// <summary>
    /// View of the <see cref="Storage"/> in the context of the current transaction.
    /// </summary>
    /// <remarks>
    /// When you make changes to entities, changes are first written in this object (i.e. in 
    /// the transaction context). They are written to the storage only when the 
    /// transaction is committed. This mechanisms provides basic transactions will
    /// rollback and 'read committed' isolation level, but without true reliability
    /// (since the commit phase may easily fail for technical reasons).
    /// </remarks>
    public sealed class StorageContext : EntityRepository, ISinglePhaseNotification
    {
        /// <summary>
        /// List of operations done in the current transaction.
        /// </summary>
        private readonly List<StorageOperation> redoLog = new List<StorageOperation>();

        /// <summary>
        /// List of entities that have been deleted in the current transaction.
        /// </summary>
        private readonly Dictionary<EntityKey, BaseEntity> deletedEntities = new Dictionary<EntityKey, BaseEntity>();

        /// <summary>
        /// Whether the current <see cref="StorageContext"/> is already enlisted in a <see cref="Transaction"/>.
        /// </summary>
        private bool enlisted;

        /// <summary>
        /// Identifier of the current instance, for debugging.
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Current transaction (each thread has its own).
        /// </summary>
        [ThreadStatic] private static StorageContext current;

        /// <summary>
        /// Initializes a new <see cref="StorageContext"/>.
        /// </summary>
        private StorageContext()
        {
            this.id = Thread.CurrentThread.ManagedThreadId;
        }


        /// <summary>
        /// Gets or sets the <see cref="StorageContext"/> of the current thread.
        /// </summary>
        public static StorageContext Current
        {
            get
            {
                if ( current == null )
                    current = new StorageContext();

                return current;
            }

            set
            {
                if ( value == null )
                    throw new ArgumentNullException( "value" );
                current = value;
            }
        }


        /// <summary>
        /// Gets an entity from the current <see cref="StorageContext"/>,
        /// and throw an exception if it is not found.
        /// </summary>
        /// <param name="entityKey">BaseEntity key.</param>
        /// <returns>The entity with this key.</returns>
        public BaseEntity GetEntity( EntityKey entityKey )
        {
            return this.GetEntity( entityKey, true );
        }

        /// <summary>
        /// Gets an entity from the current <see cref="StorageContext"/>,
        /// and specify whether an exception should be thrown if it is not found.
        /// </summary>
        /// <param name="entityKey">BaseEntity key.</param>
        /// <param name="throwIfNotFound"><b>true</b> if an exception should be
        /// thrown if the entity is not found, otherwise <b>false</b>.</param>
        /// <returns>The entity with this key.</returns>
        public BaseEntity GetEntity( EntityKey entityKey, bool throwIfNotFound )
        {
            if ( entityKey.IsNull )
                throw new ArgumentNullException( "entityKey" );

            BaseEntity entity = this.InternalGet( entityKey );

            if ( entity == null )
            {
                entity = Storage.Current.GetEntity( entityKey );
            }


            if ( entity != null )
            {
                return entity.Clone();
            }
            else if ( throwIfNotFound )
            {
                throw new RowNotInTableException();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Enlist the current instance into a transaction.
        /// </summary>
        private void Enlist()
        {
            // Require a transaction to be open.
            if ( Transaction.Current == null )
            {
                throw new InvalidOperationException( "This operation requires a transaction." );
            }


            // Enlist if not already done.
            if ( !this.enlisted )
            {
                Transaction.Current.EnlistVolatile( this, EnlistmentOptions.None );
                this.enlisted = true;
            }
        }

        /// <summary>
        /// Updates an entity to the database.
        /// </summary>
        /// <param name="entity">BaseEntity.</param>
        [Trace]
        public void Update( BaseEntity entity )
        {
            if ( entity == null )
                throw new ArgumentNullException( "entity" );
            if ( entity.EntityKey.IsNull )
                throw new ArgumentException( "Cannot update an entity that has no identifier.", "entity" );

            entity.Validate();

            this.Enlist();

            BaseEntity entityClone = entity.Clone();
            this.InternalSet( entityClone );

            if ( this.redoLog != null )
            {
                this.redoLog.Add( new StorageOperation( StorageOperationKind.Update, entityClone ) );
            }
        }


        /// <summary>
        /// Inserts a new entity to the database.
        /// </summary>
        /// <param name="entity">BaseEntity (with null entity key).</param>
        /// <returns>The <see cref="EntityKey"/> that has been assigned to this entity.</returns>
        [Trace]
        public EntityKey Insert( BaseEntity entity )
        {
            if ( entity == null )
                throw new ArgumentNullException( "entity" );
            if ( !entity.EntityKey.IsNull )
                throw new ArgumentException( "Cannot insert an entity that has an identifier.", "entity" );

            entity.Validate();

            this.Enlist();

            entity.EntityKey = Storage.MakeEntityKey();
            BaseEntity entityClone = entity.Clone();

            this.InternalSet( entityClone );

            if ( this.redoLog != null )
            {
                this.redoLog.Add( new StorageOperation( StorageOperationKind.Insert, entityClone ) );
            }


            return entity.EntityKey;
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity">Reference to the entity to be deleted.</param>
        [Trace]
        public void Delete( EntityRef<BaseEntity> entity )
        {
            if ( entity.IsNull )
                throw new ArgumentNullException( "entity" );

            this.Enlist();

            if ( this.redoLog != null )
            {
                this.redoLog.Add( new StorageOperation( StorageOperationKind.Delete, entity ) );
            }

            if ( this.deletedEntities != null )
            {
                this.deletedEntities.Add( entity.EntityKey, this.GetEntity( entity.EntityKey ) );
            }

            this.InternalRemove( entity.EntityKey );
        }

        /// <summary>
        /// Gets an enumerator of all entities that have been modified in this <see cref="StorageContext"/>.
        /// </summary>
        /// <returns>An enumerator.</returns>
        private IEnumerator<BaseEntity> GetModifiedEntitiesEnumerator()
        {
            return base.GetAllEntitiesEnumerator();
        }


        /// <summary>
        /// Gets an enumerators for all entities, not only in the current <see cref="StorageContext"/>,
        /// but in the whole database.
        /// </summary>
        /// <returns>An enumerator.</returns>
        protected override IEnumerator<BaseEntity> GetAllEntitiesEnumerator()
        {
            // Return the entities in the current context.
            IEnumerator<BaseEntity> modifiedEnumerator = this.GetModifiedEntitiesEnumerator();
            while ( modifiedEnumerator.MoveNext() )
            {
                yield return modifiedEnumerator.Current;
            }


            // If we don't have the committed version, look also
            // in the committed repository;
            IEnumerator<BaseEntity> committedEnumerator = Storage.Current.GetAllEntitiesEnumerator();
            while ( committedEnumerator.MoveNext() )
            {
                BaseEntity entity = committedEnumerator.Current;

                // Check that we did not already return it.
                if ( this.InternalGet( entity.EntityKey ) != null )
                    continue;

                // Check that we did not delete it.
                if ( this.deletedEntities.ContainsKey( entity.EntityKey ) )
                    continue;


                // We can return it.
                yield return entity;
            }
        }

        #region ISinglePhaseNotification Members

        void ISinglePhaseNotification.SinglePhaseCommit( SinglePhaseEnlistment singlePhaseEnlistment )
        {
            ( (IEnlistmentNotification) this ).Commit( singlePhaseEnlistment );
        }

        #endregion

        #region IEnlistmentNotification Members

        private void Reset()
        {
            this.redoLog.Clear();
            this.deletedEntities.Clear();
            this.InternalClear();
        }

        [Trace]
        void IEnlistmentNotification.Commit( Enlistment enlistment )
        {
            Storage.Current.DoOperations( this.redoLog );
            this.Reset();
            enlistment.Done();
            this.enlisted = false;
        }

        [Trace]
        void IEnlistmentNotification.InDoubt( Enlistment enlistment )
        {
            this.Reset();
            enlistment.Done();
            this.enlisted = false;
        }

        [Trace]
        void IEnlistmentNotification.Prepare( PreparingEnlistment preparingEnlistment )
        {
            preparingEnlistment.Prepared();
        }

        [Trace]
        void IEnlistmentNotification.Rollback( Enlistment enlistment )
        {
            this.Reset();
            enlistment.Done();
            this.enlisted = false;
        }

        #endregion

        public override string ToString()
        {
            return string.Format( "StorageContext {0}", this.id );
        }
    }
}