using System;
using System.Diagnostics;
using System.ServiceModel;

namespace ContactManager.Server
{
    internal class Program
    {
        private static void Main( string[] args )
        {
            Trace.Listeners.Add( new TextWriterTraceListener( Console.Out ) );
            ServiceHost serviceHost = new ServiceHost( typeof(EntityService) );
            serviceHost.Open();
            Console.WriteLine( "Listening. Press Enter to exit." );
            Console.ReadLine();
        }
    }
}