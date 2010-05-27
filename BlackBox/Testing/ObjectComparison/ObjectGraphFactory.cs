// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using BlackBox.Testing;

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
        /// <param name="typePropertiesToIgnore"></param>
        /// <param name="instancePropertiesToIgnore">Properties to exclude from the comparison, i.e. properties which parents will be leaf nodes</param>
        /// <returns>The root node of the created graph.</returns>
        public virtual GraphNode CreateObjectGraph(object value,
                                                   IEnumerable<MemberInfo> typePropertiesToIgnore,
                                                   Dictionary<object, List<MemberInfo>> instancePropertiesToIgnore,
                                                   IEnumerable<PropertyComparator> customTypePropertyComparisons,
                                                   Dictionary<object, List<PropertyComparator>> customInstancePropertyComparisons)
        {
            throw new NotSupportedException("Please provide a behavior for this method in a derived class");
        }
    }
}

