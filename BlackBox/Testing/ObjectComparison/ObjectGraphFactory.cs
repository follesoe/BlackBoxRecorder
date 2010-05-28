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
        protected List<MemberInfo> HaltOnTypeMember { get; set; }
        protected Dictionary<object, List<MemberInfo>> HaltOnInstanceMember { get; set; }

        protected ObjectGraphFactory()
        {
            HaltOnTypeMember = new List<MemberInfo>();
            HaltOnInstanceMember = new Dictionary<object, List<MemberInfo>>();
        }

        public void HaltTraversalOn(MemberInfo memberInfo)
        {
            HaltOnTypeMember.Add(memberInfo);
        }

        public void HaltTraversalOnInstance(object instance, MemberInfo memberInfo)
        {
            if(!HaltOnInstanceMember.ContainsKey(instance))
                HaltOnInstanceMember.Add(instance, new List<MemberInfo>());
            HaltOnInstanceMember[instance].Add(memberInfo);
        }

        /// <summary>
        /// Creates a graph for the given object.
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <returns>The root node of the created graph.</returns>
        public virtual GraphNode CreateObjectGraph(object value)
        {
            throw new NotSupportedException("Please provide a behavior for this method in a derived class");
        }
    }
}

