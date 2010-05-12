using System.Collections.Generic;
using System.Linq;
using BlackBox.Recorder;

namespace BlackBox.Demo.App.WebDependencies
{
    public class MyAirports
    {
        private FlightDataService _service;
        private Dictionary<string, string> _airports;
        
        public MyAirports()
        {
            _service = new FlightDataService();
        }

        [Recording]
        public List<Flight> GetDepartures(string city)
        {
            if (_airports == null)
                _airports = _service.GetAirports();

            string airportCode = string.Empty;
            foreach(var possibleCity in _airports)
            {
                if(possibleCity.Value == city)
                    airportCode = possibleCity.Key;
            }

            return _service.GetDepartures(airportCode).ToList();
        }
    }
}
