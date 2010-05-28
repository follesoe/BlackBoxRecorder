// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BlackBox.Testing;

namespace Microsoft.Test.ObjectComparison
{
    /// <summary>
    /// Creates a graph by extracting public instance properties in the object. If the
    /// property is an IEnumerable, extract the items. If an exception is thrown
    /// when accessing a property on the left object, it is considered a match if
    /// the same exception type is thrown when accessing the property on the right
    /// object.
    /// </summary>
    public sealed class PublicPropertyObjectGraphFactory : ObjectGraphFactory
    {
        /// <summary>
        /// Creates a graph for the given object by extracting public properties.
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <returns>The root node of the created graph.</returns>
        public override GraphNode CreateObjectGraph(object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            GraphNode root = CreateRoot(value);
            Queue<GraphNode> pendingQueue = CreatePendingQueue(root);
            var visitedObjects = new Dictionary<int, GraphNode>();

            while (pendingQueue.Count != 0)
            {
                GraphNode currentNode = pendingQueue.Dequeue();
                object nodeData = currentNode.ObjectValue;
                Type nodeType = currentNode.ObjectType;

                if (IsLeafNode(nodeData, nodeType))
                    continue;

                if (visitedObjects.Keys.Contains(nodeData.GetHashCode()))
                {
                    // Caused by a cycle - we have alredy seen this node so
                    // use the existing node instead of creating a new one
                    GraphNode prebuiltNode = visitedObjects[nodeData.GetHashCode()];
                    currentNode.Children.Add(prebuiltNode);
                    continue;
                }
                visitedObjects.Add(nodeData.GetHashCode(), currentNode);

                IEnumerable<GraphNode> childNodes = GetChildNodes(nodeData);
                foreach (GraphNode childNode in childNodes)
                {
                    childNode.Parent = currentNode;
                    currentNode.Children.Add(childNode);
                    pendingQueue.Enqueue(childNode);
                }
            }
            return root;
        }

        private static IEnumerable<GraphNode> GetChildNodes(object nodeData)
        {
            var childNodes = new Collection<GraphNode>();
            foreach (GraphNode child in ExtractProperties(nodeData))
                childNodes.Add(child);

            // Extract and add IEnumerable content
            if (IsIEnumerable(nodeData))
                foreach (GraphNode child in GetIEnumerableChildNodes(nodeData))
                    childNodes.Add(child);

            return childNodes;
        }

        private static IEnumerable<GraphNode> ExtractProperties(object nodeData)
        {
            if (IsIEnumerable(nodeData))
                return new List<GraphNode>();

            IEnumerable<PropertyInfo> properties = GetPublicInstanceProperties(nodeData);
            return from property in properties
                   let parameters = property.GetIndexParameters()
                   where property.CanRead && parameters.Length == 0
                   let value = GetValue(nodeData, property)
                   select new GraphNode
                              {
                                  Name = property.Name,
                                  ObjectValue = value,
                                  ObjectType = property.PropertyType,
                                  Property = property
                              };
        }

        private static object GetValue(object nodeData, PropertyInfo property)
        {
            try
            {
                return property.GetValue(nodeData, null);
            }
            catch (Exception ex)
            {
                // If accessing the property threw an exception
                // then make the type of exception as the child.
                // Do we want to validate the entire exception object
                // here ? - currently not doing to improve perf.
                return ex.GetType().ToString();
            }
        }

        private static IEnumerable<PropertyInfo> GetPublicInstanceProperties(object nodeData)
        {
            return nodeData.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        private static IEnumerable<GraphNode> GetIEnumerableChildNodes(object nodeData)
        {
            var childNodes = new List<GraphNode>();
            var enumerableData = nodeData as IEnumerable;
            IEnumerator enumerator = enumerableData.GetEnumerator();
            int count = 0;
            while (enumerator.MoveNext())
                childNodes.Add(new GraphNode
                                   {
                                       Name = "IEnumerable" + count++,
                                       ObjectValue = enumerator.Current,
                                       ObjectType = enumerator.Current.GetType()
                                   });
            return childNodes;
        }

        private static bool IsIEnumerable(object nodeData)
        {
            var enumerableData = nodeData as IEnumerable;
            return enumerableData != null &&
                   enumerableData.GetType().IsPrimitive == false &&
                   nodeData.GetType() != typeof (System.String);
        }

        private static bool IsLeafNode(object nodeData, Type nodeType)
        {
            return nodeData == null ||
                   nodeType.IsPrimitive ||
                   nodeType.IsValueType ||
                   nodeType == typeof(string);
        }

        private static Queue<GraphNode> CreatePendingQueue(GraphNode root)
        {
            var queue = new Queue<GraphNode>();
            queue.Enqueue(root);
            return queue;
        }

        private static GraphNode CreateRoot(object value)
        {
            return new GraphNode()
            {
                Name = value is IEnumerable ? "" : value.GetType().Name,
                ObjectValue = value,
                ObjectType = value.GetType()
            };
        }
    }
}

