using System;

namespace ContactManager
{
    public static class Populate
    {
        private const string contacts =
            @"
Magora Wolfswift
Ricgold Dryadson
Breatra Sandtracker
Jamendithas Silverkin
Stohana Stalkingwolf
Wilros Windsailor
Wilven Duskwalker
Jamlamin Gladdenstone
Horlamin Rubymace
Brethyra Swordhand
Xantumal Dagarkin
Yengretor Steelfarmer
Ravalove Huntinghawk
";

        private const string countries =
            @"
Afghanistan
Albania
Algeria
Andorra
Angola
Antigua and Barbuda
Argentina
Armenia
Australia
Austria
Azerbaijan
Bahamas, The
Bahrain
Bangladesh
Barbados
Belarus
Belgium
Belize
Benin
Bhutan
Bolivia
Bosnia and Herzegovina
Botswana
Brazil
Brunei
Bulgaria
Burkina Faso
Burundi
Cambodia
Cameroon
Canada
Cape Verde
Central African Republic
Chad
Chile
China, People's Republic of
Colombia
Comoros
Congo, Democratic Republic of the (Congo – Kinshasa)
Congo, Republic of the (Congo – Brazzaville)
Costa Rica
Cote d'Ivoire (Ivory Coast)
Croatia
Cuba
Cyprus
Czech Republic
Denmark
Djibouti
Dominica
Dominican Republic
Ecuador
Egypt
El Salvador
Equatorial Guinea
Eritrea
Estonia
Ethiopia
Fiji
Finland
France
Gabon
Gambia, The
Georgia
Germany
Ghana
Greece
Grenada
Guatemala
Guinea
Guinea-Bissau
Guyana
Haiti
Honduras
Hungary
Iceland
India
Indonesia
Iran
Iraq
Ireland
Israel
Italy
Jamaica
Japan
Jordan
Kazakhstan
Kenya
Kiribati
Korea, Democratic People's Republic of (North Korea)
Korea, Republic of  (South Korea)
Kuwait
Kyrgyzstan
Laos
Latvia
Lebanon
Lesotho
Liberia
Libya
Liechtenstein
Lithuania
Luxembourg
Macedonia
Madagascar
Malawi
Malaysia
Maldives
Mali
Malta
Marshall Islands
Mauritania
Mauritius
Mexico
Micronesia
Moldova
Monaco
Mongolia
Montenegro
Morocco
Mozambique
Myanmar (Burma)
Namibia
Nauru
Nepal
Netherlands
New Zealand
Nicaragua
Niger
Nigeria
Norway
Oman
Pakistan
Palau
Panama
Papua New Guinea
Paraguay
Peru
Philippines
Poland
Portugal
Qatar
Romania
Russia
Rwanda
Saint Kitts and Nevis
Saint Lucia
Saint Vincent and the Grenadines
Samoa
San Marino
Sao Tome and Principe
Saudi Arabia
Senegal
Serbia
Seychelles
Sierra Leone
Singapore
Slovakia
Slovenia
Solomon Islands
Somalia
South Africa
Spain
Sri Lanka
Sudan
Suriname
Swaziland
Sweden
Switzerland
Syria
Tajikistan
Tanzania
Thailand
Timor-Leste (East Timor)
Togo
Tonga
Trinidad and Tobago
Tunisia
Turkey
Turkmenistan
Tuvalu
Uganda
Ukraine
United Arab Emirates
United Kingdom
United States
Uruguay
Uzbekistan
Vanuatu
Vatican City
Venezuela
Vietnam
Yemen
Zambia
Zimbabwe
Abkhazia
China, Republic of (Taiwan)
Nagorno-Karabakh
Northern Cyprus
Pridnestrovie (Transnistria)
Somaliland
South Ossetia
Ashmore and Cartier Islands
Christmas Island
Cocos (Keeling) Islands
Coral Sea Islands
Heard Island and McDonald Islands
Norfolk Island
New Caledonia
French Polynesia
Mayotte
Saint Barthelemy
Saint Martin
Saint Pierre and Miquelon
Wallis and Futuna
French Southern and Antarctic Lands
Clipperton Island
Bouvet Island
Cook Islands
Niue
Tokelau
Guernsey
Isle of Man
Jersey
Anguilla
Bermuda
British Indian Ocean Territory
British Sovereign Base Areas
British Virgin Islands
Cayman Islands
Falkland Islands (Islas Malvinas)
Gibraltar
Montserrat
Pitcairn Islands
Saint Helena
South Georgia and the South Sandwich Islands
Turks and Caicos Islands
Northern Mariana Islands
Puerto Rico
American Samoa
Baker Island
Guam
Howland Island
Jarvis Island
Johnston Atoll
Kingman Reef
Midway Islands
Navassa Island
Palmyra Atoll
U.S. Virgin Islands
Wake Island
Hong Kong
Macau
Faroe Islands
Greenland
French Guiana
Guadeloupe
Martinique
Reunion
Aland
Aruba
Netherlands Antilles
Svalbard
Ascension
Tristan da Cunha
Antarctica
Kosovo
Palestinian Territories (Gaza Strip and West Bank)
Western Sahara
Australian Antarctic Territory
Ross Dependency
Peter I Island
Queen Maud Land
British Antarctic Territory
";

        public static string[] GetCountries()
        {
            return countries.Split( new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries );
        }

        public static string[] GetContacts()
        {
            return contacts.Split( new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries );
        }
    }
}