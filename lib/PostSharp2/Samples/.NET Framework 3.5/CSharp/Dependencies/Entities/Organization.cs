using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Dependencies.Aspects;

namespace Dependencies.Entities
{
    class Organization : ISecurable
    {
        readonly AccessList accessList = new AccessList();


        public AccessList AccessList { get { return this.accessList; } }

        // Uncomment this if you want a build-time error.
        //[Cache]
        public bool IsUserInRole( IPrincipal principal, string role )
        {
            return accessList.IsUserInRole(principal, role);
        }


    }
}