using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using ContactManager.Entities;
using ContactManager.Interface;
using PostSharp;
using Threading;

namespace ContactManager.Framework
{
    [ReaderWriterSynchronized]
    internal class Client : IEntityCallback
    {
        private readonly IEntityService service;
        private readonly WeakDictionary<Guid, IObservableFinalize> entities = new WeakDictionary<Guid, IObservableFinalize>();

        private static readonly Dictionary<Type, EntityType> entityTypes = new Dictionary<Type, EntityType>
                                                                               {
                                                                                   {typeof(Contact), EntityType.Contact},
                                                                                   {typeof(Country), EntityType.Country}
                                                                               };

        private static readonly Dictionary<EntityType, Type> entityTypesReverse = new Dictionary<EntityType, Type>();
        private readonly ServiceClient serviceClient;

        static Client()
        {
            foreach ( KeyValuePair<Type, EntityType> pair in entityTypes )
            {
                entityTypesReverse.Add( pair.Value, pair.Key );
            }
        }

        private Client()
        {
            this.serviceClient = new ServiceClient( new InstanceContext( this ) );
            this.service = this.serviceClient.ChannelFactory.CreateChannel();
        }

        public static Client Current { get; private set; }

        public static void Initialize()
        {
            Current = new Client();
        }


        [WriteLock]
        public T[] GetEntities<T>() where T : Entity
        {
            EntityType entityType = entityTypes[typeof(T)];

            IList<EntityData> dataList = this.service.GetEntities( entityType );
            T[] newEntities = new T[dataList.Count];

            for ( int i = 0; i < newEntities.Length; i++ )
            {
                newEntities[i] = (T) GetEntity( dataList[i], typeof(T) );
            }

            return newEntities;
        }

        private Entity GetEntity( EntityData data, Type entityType )
        {
            // Check if we already have this entity.
            Entity entity;
            IObservableFinalize value;
            if ( this.entities.TryGetValue( data.Id, out value ) )
            {
                entity = (Entity) value;
                entity.OnUpdated( data );
                return entity;
            }
            else
            {
                entity = (Entity) FormatterServices.GetSafeUninitializedObject( entityType );
                entity.InitializeFromData( data );
                RegisterEntity( entity );
                return entity;
            }
        }

        private void EntityOnDisposed( object sender, EventArgs args )
        {
            Entity entity = (Entity) sender;
            UnregisterEntity( entity );
        }

        internal void UpdateEntity( Entity entity )
        {
            this.service.UpdateEntity( entity.Data );
        }

        [WriteLock]
        internal void CreateEntity( Entity entity )
        {
            this.service.UpdateEntity( entity.Data );
            this.RegisterEntity( entity );
        }

        [WriteLock]
        internal void DeleteEntity( Entity entity )
        {
            this.service.DeleteEntity( entity.Id );
            this.UnregisterEntity( entity );
        }

        [WriteLock]
        private void RegisterEntity( Entity entity )
        {
            entity.Disposed += EntityOnDisposed;
            entities.Add( entity.Id, Post.Cast<Entity, IObservableFinalize>( entity ) );
        }

        [WriteLock]
        private void UnregisterEntity( Entity entity )
        {
            entity.Disposed -= EntityOnDisposed;
            entities.Remove( entity.Id );
        }

        [WriteLock]
        void IEntityCallback.OnEntityDeleted( Guid id )
        {
            IObservableFinalize entity;
            if ( this.entities.TryGetValue( id, out entity ) )
            {
                ((Entity) entity).OnDeleted();
            }
        }

        [ReadLock]
        void IEntityCallback.OnEntityUpdated( EntityData entityData )
        {
            IObservableFinalize entity;
            if ( this.entities.TryGetValue( entityData.Id, out entity ) )
            {
                ((Entity) entity).OnUpdated( entityData );
            }
        }

        [WriteLock]
        void IEntityCallback.OnEntityCreated( EntityData entityData )
        {
            if ( this.EntityCreated != null )
            {
                IObservableFinalize value;
                Entity entity;
                if ( !this.entities.TryGetValue( entityData.Id, out value ) )
                {
                    Type entityType = entityTypesReverse[entityData.EntityType];
                    entity = GetEntity( entityData, entityType );
                }
                else
                {
                    entity = (Entity) value;
                }


                this.EntityCreated( this, new EntityEventArgs( entity ) );
            }
        }

        [ReadLock]
        public event EventHandler<EntityEventArgs> EntityCreated;

        private class ServiceClient : ClientBase<IEntityService>
        {
            public ServiceClient( InstanceContext context ) : base( context, new NetTcpBinding {MaxConnections = 100},
                                                                    new EndpointAddress( "net.tcp://localhost:1000/EntityService" ) )
            {
            }
        }
    }

    public class EntityEventArgs : EventArgs
    {
        public EntityEventArgs( Entity entity )
        {
            this.Entity = entity;
        }

        public Entity Entity { get; private set; }
    }
}