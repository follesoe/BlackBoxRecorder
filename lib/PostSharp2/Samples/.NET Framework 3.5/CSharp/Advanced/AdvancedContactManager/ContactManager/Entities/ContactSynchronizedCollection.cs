using System;
using System.ComponentModel;
using ContactManager.Framework;
using Threading;

namespace ContactManager.Entities
{
    public class ContactSynchronizedCollection : SynchronizedKeyedCollection<Guid, Contact>
    {
        public static readonly ContactSynchronizedCollection Instance = new ContactSynchronizedCollection();

        private ContactSynchronizedCollection()
        {
            Client.Current.EntityCreated += OnEntityCreated;
            foreach ( Contact contact in Client.Current.GetEntities<Contact>() )
            {
                this.AddContact( contact );
            }
        }

        private void AddContact( Contact contact )
        {
            contact.PropertyChanged += OnContactChanged;
            this.Add( contact );
        }

        private void OnContactChanged( object sender, PropertyChangedEventArgs e )
        {
            Contact contact = (Contact) sender;
            if ( contact != null && (e.PropertyName == null || e.PropertyName == "EntityStatus") && contact.EntityStatus == EntityStatus.Deleted )
            {
                contact.PropertyChanged -= OnContactChanged;
                this.Remove( contact );
            }
        }

        private void OnEntityCreated( object sender, EntityEventArgs e )
        {
            Contact contact = e.Entity as Contact;
            if ( contact == null ) return;

            this.AddContact( contact );
        }

        protected override Guid GetKeyForItem( Contact item )
        {
            return item.Id;
        }
    }
}