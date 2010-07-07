using System;
using System.ServiceModel;
using ContactManager.Interface;

namespace ContactManager.Server
{
    [ServiceBehavior( InstanceContextMode = InstanceContextMode.PerSession )]
    public class EntityService : IEntityService, IDisposable
    {
        private IEntityCallback callback;

        public EntityService()
        {
            this.callback = OperationContext.Current.GetCallbackChannel<IEntityCallback>();
            Database.Instance.EntityCreated += OnEntityCreated;
            Database.Instance.EntityUpdated += OnEntityUpdated;
            Database.Instance.EntityDeleted += OnEntityDeleted;
        }

        private void OnEntityDeleted( object sender, EntityIdEventArgs e )
        {
            if ( this.callback != null )
            {
                try
                {
                    this.callback.OnEntityDeleted( e.EntityId );
                }
                catch
                {
                    this.callback = null;
                }
            }
        }

        private void OnEntityUpdated( object sender, EntityEventArgs e )
        {
            if ( this.callback != null )
            {
                try
                {
                    this.callback.OnEntityUpdated( e.Entity );
                }
                catch
                {
                    this.callback = null;
                }
            }
        }

        private void OnEntityCreated( object sender, EntityEventArgs e )
        {
            if ( this.callback != null )
            {
                try
                {
                    this.callback.OnEntityCreated( e.Entity );
                }
                catch
                {
                    this.callback = null;
                }
            }
        }


        public void UpdateEntity( EntityData entity )
        {
            Database.Instance.UpdateEntity( entity );
        }

        public void DeleteEntity( Guid entityId )
        {
            Database.Instance.DeleteEntity( entityId );
        }

        public EntityData[] GetEntities( EntityType entityType )
        {
            return Database.Instance.GetEntities( entityType );
        }

        public void Dispose()
        {
            Database.Instance.EntityCreated -= OnEntityCreated;
            Database.Instance.EntityUpdated -= OnEntityUpdated;
            Database.Instance.EntityDeleted -= OnEntityDeleted;
        }
    }
}