using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
namespace Dependencies.Aspects
{

    // This aspect provides no advise at all, but provides a dependency: it cannot
    // be found together on the same method with a caching aspect.
    [AspectRoleDependency(AspectDependencyAction.Conflict, StandardRoles.Caching)]
    class NotCacheableAttribute : MethodLevelAspect
    {
    }
}
