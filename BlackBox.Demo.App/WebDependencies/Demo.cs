using System;

namespace BlackBox.Demo.App.WebDependencies
{
    public class Demo
    {
        public static void Run()
        {
            var myAirports = new MyAirports();

            Console.WriteLine("Departures from Lakselv");
            foreach (var departure in myAirports.GetDepartures("Lakselv"))
                Console.WriteLine("\t{0}\t{1}\t{2}", departure.FlightNumber, departure.Airport, departure.Time);

            Console.WriteLine();

            Console.WriteLine("Departures from Alta");
            foreach (var departure in myAirports.GetDepartures("Alta"))
                Console.WriteLine("\t{0}\t{1}\t{2}", departure.FlightNumber, departure.Airport, departure.Time);

            Console.WriteLine();
        }
    }
}
