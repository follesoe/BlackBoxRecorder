using System;
using ContactManager.Framework;
using ContactManager.Interface;
using Threading;

namespace ContactManager.Entities
{
    public class Contact : Entity
    {
        public Contact() : base( new ContactData {EntityType = EntityType.Contact} )
        {
        }

        private new ContactData Data
        {
            get { return (ContactData) base.Data; }
        }

        public string FirstName
        {
            get { return this.Data.FirstName; }
            set { this.Data.FirstName = value; }
        }

        public string LastName
        {
            get { return this.Data.LastName; }
            set { this.Data.LastName = value; }
        }

        public Guid CountryId
        {
            get { return this.Data.CountryId; }
            set { this.Data.CountryId = value; }
        }

        public string FullName
        {
            [ReadLock]
            get { return this.FirstName + " " + this.LastName; }
        }
    }
}