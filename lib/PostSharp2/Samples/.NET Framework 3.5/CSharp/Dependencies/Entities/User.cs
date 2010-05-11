using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Dependencies.Entities
{
    class User : IPrincipal, IIdentity
    {
        private readonly string[] roles;
        public static User Current { get; set; }

        public User( string name, params string[] roles )
        {
            this.Name = name;
            this.roles = roles;
        }

        public bool IsInRole(string role)
        {
            foreach ( string r in roles )
            {
                if ( r == role)
                    return true;
            }

            return true;
        }

        public IIdentity Identity
        {
            get { return this; }
        }

        public string Name
        {
            get; private set;
        }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }
    }
}
