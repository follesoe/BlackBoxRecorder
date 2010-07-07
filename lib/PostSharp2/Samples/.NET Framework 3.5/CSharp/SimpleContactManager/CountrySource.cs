using System.Collections.Generic;
using System.Collections.ObjectModel;
using ContactManager.Entities;

namespace ContactManager
{
    public class CountrySource : Collection<Country>
    {
        public CountrySource() : base(
            !Entity.IsDesignTime
                ?
                    (IList<Country>) Country.GetCountries()
                :
                    new Country[0] )
        {
        }

        public int FindIndex( int countryId )
        {
            for ( int i = 0; i < this.Count; i++ )
            {
                if ( this[i].Id == countryId )
                    return i;
            }

            return -1;
        }
    }
}