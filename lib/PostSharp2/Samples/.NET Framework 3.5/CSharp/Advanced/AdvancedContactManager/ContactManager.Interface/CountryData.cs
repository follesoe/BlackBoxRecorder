using System.Runtime.Serialization;

namespace ContactManager.Interface
{
    [DataContract]
    public class CountryData : EntityData
    {
        public CountryData() : base( EntityType.Country )
        {
        }

        [DataMember]
        public string Name { get; set; }
    }
}