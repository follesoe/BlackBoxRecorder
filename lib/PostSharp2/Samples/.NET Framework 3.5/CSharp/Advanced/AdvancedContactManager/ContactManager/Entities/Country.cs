using System.Collections.Generic;
using ContactManager.Framework;
using ContactManager.Interface;

namespace ContactManager.Entities
{
    public class Country : Entity
    {
        public Country() : base( new CountryData {EntityType = EntityType.Country} )
        {
        }

        private new CountryData Data
        {
            get { return (CountryData) base.Data; }
        }

        public string Name
        {
            get { return this.Data.Name; }
        }

        internal static IList<Country> GetCountries()
        {
            return Client.Current.GetEntities<Country>();
        }
    }
}