using System;
using System.ComponentModel;
using ContactManager.Interface;
using Threading;

namespace ContactManager.Framework
{
    [ReaderWriterSynchronized]
    [UpdateEntity]
    [ThrowWhenDisposed]
    [ObservableFinalizeAspect]
    [NotifyPropertyChanged]
    public abstract class Entity : IDisposable, INotifyPropertyChanged
    {
        private Guid id;
        private EntityData data;

        protected Entity( EntityData data )
        {
            if ( data.EntityType == EntityType.Unknown )
                throw new ArgumentOutOfRangeException();

            this.id = Guid.NewGuid();
            this.data = data;
            this.data.Id = this.id;
            this.entityStatus = EntityStatus.New;
        }

        protected virtual void InitializeAspects()
        {
        }

        [SafeWhenDisposed]
        internal void InitializeFromData( EntityData data )
        {
            this.InitializeAspects();
            this.data = data;
            this.id = data.Id;
            this.entityStatus = EntityStatus.Clean;
        }


        protected internal EntityData Data
        {
            get { return this.data; }
        }

        public Guid Id
        {
            [SafeWhenDisposed]
            get { return this.id; }
        }

        private EntityStatus entityStatus;

        public EntityStatus EntityStatus
        {
            get { return entityStatus; }
        }


        [WriteLock]
        public void Delete()
        {
            if ( this.EntityStatus == EntityStatus.New )
                throw new InvalidOperationException();

            this.entityStatus = EntityStatus.Deleted;
            Client.Current.DeleteEntity( this );

            this.OnPropertyChanged( "EntityStatus" );
        }

        [WriteLock]
        public void Save()
        {
            switch ( this.EntityStatus )
            {
                case EntityStatus.Dirty:
                    Client.Current.UpdateEntity( this );
                    this.entityStatus = EntityStatus.Clean;
                    break;

                case EntityStatus.New:
                    Client.Current.CreateEntity( this );
                    this.entityStatus = EntityStatus.Clean;
                    break;

                case EntityStatus.Deleted:
                case EntityStatus.Conflict:
                    throw new InvalidOperationException();
            }
        }

        #region Dispose

        public event EventHandler Disposed;

        public bool IsDisposed { [SafeWhenDisposed]
        get; private set; }

        [SafeWhenDisposed]
        public virtual void Dispose()
        {
            if ( !this.IsDisposed )
            {
                this.IsDisposed = true;

                if ( this.Disposed != null )
                    this.Disposed( this, EventArgs.Empty );
            }
        }

        #endregion

        internal void OnDeleted()
        {
            this.entityStatus = EntityStatus.Deleted;
        }

        [WriteLock]
        internal void OnUpdated( EntityData data )
        {
            switch ( this.EntityStatus )
            {
                case EntityStatus.Dirty:
                    this.entityStatus = EntityStatus.Conflict;
                    this.OnPropertyChanged("EntityStatus");
                    return;
            }

            this.data = data;
            this.OnPropertyChanged( null );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [ObserverLock]
        protected virtual void OnPropertyChanged( string propertyName )
        {
            if ( this.PropertyChanged != null )
            {
                // We always ignore the property name since computed properties may change also.
                this.PropertyChanged( this, new PropertyChangedEventArgs( null ) );
            }
        }

        protected void OnEntityPropertyUpdated()
        {
            switch ( this.EntityStatus )
            {
                case EntityStatus.Clean:
                    this.data.Revision++;
                    this.entityStatus = EntityStatus.Dirty;
                    break;
            }
        }
    }
}