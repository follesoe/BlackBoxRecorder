// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using System.Reflection;

namespace Microsoft.Test.ObjectComparison
{
    [DebuggerDisplay("{Name}")]
    public class GraphNode
    {
        //public bool Allow { get; set; }
        public Collection<GraphNode> Children { get; private set; }
        public string Name { get; set; }
        public bool Ignore { get; private set; }
        public object ObjectValue { get; set; }
        public Type ObjectType { get; set; }
        public GraphNode Parent { get; set; }
        public PropertyInfo Property { get; set; }

        public int Depth
        {
            get
            {
                GraphNode node = this;
                int depth = 0;
                while (node.Parent != null)
                {
                    depth++;
                    node = node.Parent;
                }
                return depth;
            }
        }

        public string QualifiedName
        {
            get
            {
                GraphNode node = this;
                string qualifiedName = Name;
                string delimiter = ".";
                while (node.Parent != null)
                {
                    if (node.Parent.Name == "")
                        delimiter = "";
                    qualifiedName = node.Parent.Name + delimiter + qualifiedName;
                    node = node.Parent;
                }
                return qualifiedName;
            }
        }

        public GraphNode()
        {
            Children = new Collection<GraphNode>();
        }

        public void IgnoreChild(MemberInfo property)
        {
            Children.Where(c => c.Property == property)
                    .ToList()
                    .ForEach(c =>
                                 {
                                     c.Ignore = true;
                                     c.IgnoreChildren();
                                 });
        }

        public void IgnoreChildren()
        {
            Children.ToList()
                    .ForEach(c =>
                                 {
                                     c.Ignore = true;
                                     c.IgnoreChildren();
                                 });
        }

        /// <summary>
        /// Performs a depth-first traversal of the graph with 
        /// this node as root, and provides the nodes visited, in that
        /// order.
        /// </summary>
        /// <returns>Nodes visited in depth-first order.</returns>
        public IEnumerable<GraphNode> GetNodesInDepthFirstOrder()
        {
            var pendingNodes = new Stack<GraphNode>();
            pendingNodes.Push(this);
            var visitedNodes = new HashSet<GraphNode>();
            while (pendingNodes.Count != 0)
            {
                GraphNode currentNode = pendingNodes.Pop();
                if (!visitedNodes.Contains(currentNode))
                {
                    foreach (GraphNode node in currentNode.Children)
                    {
                        pendingNodes.Push(node);
                    }
                    visitedNodes.Add(currentNode);
                    yield return currentNode;
                }
            }
        }
    }
}