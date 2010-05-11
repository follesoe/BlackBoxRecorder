using System;
using System.Runtime.Serialization;

namespace ContactManager.Interface
{
    [DataContract]
    public class ContactData : EntityData
    {
        public ContactData() : base( EntityType.Contact )
        {
        }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public Guid CountryId { get; set; }
    }
}