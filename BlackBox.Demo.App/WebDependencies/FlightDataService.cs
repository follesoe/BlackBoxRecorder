using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;
using BlackBox.Recorder;

namespace BlackBox.Demo.App.WebDependencies
{
    [Dependency]
    public class FlightDataService
    {
        public Dictionary<string, string> GetAirports()
        {
            var xml = XDocument.Load("http://flydata.avinor.no/airportNames.asp");

            return xml.XPathSelectElements("//airportName")
                        .ToDictionary(airportNode => airportNode.Attribute("code").Value, 
                                      airportNode => airportNode.Attribute("name").Value);
        }

        public List<Flight> GetDepartures(string airportCode)
        {
            var xml = XDocument.Load(string.Format("http://flydata.avinor.no/XmlFeed.asp?TimeFrom=1&TimeTo=7&airport={0}&direction=D&lastUpdate=2009-03-10T15:03:00", airportCode));

            return (from node in xml.XPathSelectElements("//flight")
                   select new Flight
                              {
                                  FlightNumber = node.Element("flight_id").Value,
                                  Time = Convert.ToDateTime(node.Element("schedule_time").Value),
                                  Airport = node.Element("airport").Value
                              }).ToList();
                    
        }       
    }
}
