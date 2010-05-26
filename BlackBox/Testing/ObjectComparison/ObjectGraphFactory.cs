// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Test.ObjectComparison
{
    /// <summary>
    /// Creates a graph for the 
    /// provided object.
    /// </summary>
    public abstract class ObjectGraphFactory
    {
        /// <summary>
        /// Creates a graph for the given object.
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <param name="propertiesToIgnore">Properties to exclude from the comparison, i.e. properties which parents will be leaf nodes</param>
        /// <returns>The root node of the created graph.</returns>
<<<<<<< HEAD
        public virtual GraphNode CreateObjectGraph(object value,
                                                   IEnumerable<MemberInfo> typePropertiesToIgnore,
                                                   Dictionary<object, List<MemberInfo>> objectPropertiesToIgnore)
=======
        public virtual GraphNode CreateObjectGraph(object value, IEnumerable<MemberInfo> propertiesToIgnore)
>>>>>>> c8bb31f489161031b89e5649a4c57a760e58c337
        {
            throw new NotSupportedException("Please provide a behavior for this method in a derived class");
        }
    }
}
