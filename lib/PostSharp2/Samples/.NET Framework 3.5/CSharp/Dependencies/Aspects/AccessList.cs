using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using Dependencies.Entities;

namespace Dependencies.Aspects
{
    class AccessList 
    {
        private readonly Dictionary<IPrincipal, List<string>> users = new Dictionary<IPrincipal, List<string>>();

        public void AddRole(IPrincipal user, string role)
        {
            // We need to secure AddRole, but we can't use ISecurable
            // otherwise we would have an egg-or-chicken problem.
            if (!user.IsInRole("Administrator"))
                throw new SecurityException();

            List<string> roles;
            if ( !users.TryGetValue( user, out roles ))
            {
                roles = new List<string>();
                users.Add( user, roles );
            }

            roles.Add( role );
        }

        public bool IsUserInRole(IPrincipal user, string role)
        {
            List<string> roles;
            if (!users.TryGetValue(user, out roles))
            {
                return false;
            }
            return roles.Contains( role );
        }


    }
}
