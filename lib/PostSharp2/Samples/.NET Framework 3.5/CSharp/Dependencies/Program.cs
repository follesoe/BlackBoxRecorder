using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using Dependencies.Entities;

namespace Dependencies
{
    class Program
    {
        static void Main(string[] args)
        {
            // Populate our object model.
            User.Current = new User( "Administrator", "Administrator" );
            User joe = new User( "Joe" );
            User jack = new User( "Jack");
            
            Organization organization = new Organization();
            organization.AccessList.AddRole( joe, "SalesManager" );
            Quote quote = new Quote("1", organization);

            // Operate as Joe. He has rights.
            User.Current = joe;
            string details = quote.GetQuoteDetailsHtml();

            // If I get it a second time, it come from cache.
            quote.GetQuoteDetailsHtml();

            // Now operate as Jack. It must not work.
            User.Current = jack;
            try
            {
                details = quote.GetQuoteDetailsHtml();
                Console.WriteLine("It should never get here.");
            }
            catch(SecurityException)
            {
                Console.WriteLine("Allright. Jack has no right.");
            }


        }
    }
}
