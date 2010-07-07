using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Dependencies.Aspects;

namespace Dependencies.Entities
{
    class Quote : ISecurable
    {
        public Quote(string id, Organization organization)
        {
            this.Id = id;
            this.Organization = organization;
        }

        public string Id { get; private set; }

        public Organization Organization { get; private set; }

        public bool IsUserInRole( IPrincipal principal, string role )
        {
            return this.Organization.IsUserInRole( principal, role );
        }

        [Authorization("SalesManager")]
        [Cache]
        //[Trace]
        public string GetQuoteDetailsHtml()
        {
            Console.WriteLine("Pretend we are consuming expensive resources here.");
            // Pretend we retrieve the quote details, an HTML document, from database.
            // We want to cache and ensure only sales managers of this organization
            // can see quote details.
            return "<html><body>You'll get it for 100 bucks.</body></html>";
        }
    }
}