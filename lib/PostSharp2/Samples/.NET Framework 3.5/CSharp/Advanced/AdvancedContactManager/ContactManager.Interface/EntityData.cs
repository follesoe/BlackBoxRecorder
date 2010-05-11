using System;
using System.Runtime.Serialization;

namespace ContactManager.Interface
{
    [DataContract]
    [KnownType( typeof(ContactData) )]
    [KnownType( typeof(CountryData) )]
    public abstract class EntityData
    {
        protected EntityData( EntityType type )
        {
            this.EntityType = type;
        }

        [DataMember]
        public EntityType EntityType { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int Revision { get; set; }
    }
}