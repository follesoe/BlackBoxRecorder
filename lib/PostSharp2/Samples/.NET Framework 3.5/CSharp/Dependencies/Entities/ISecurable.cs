using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Dependencies.Aspects;
using Dependencies.Entities;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace Dependencies.Entities
{
    public interface ISecurable
    {
        // We never ever want this method to be cached, so we put a NotCacheable aspect
        // and specify this aspect should be inherited to all methods implementing
        // this semantic.
        [NotCacheable(AttributeInheritance = MulticastInheritance.Strict)]
        bool IsUserInRole( IPrincipal principal, string role );
    }
}