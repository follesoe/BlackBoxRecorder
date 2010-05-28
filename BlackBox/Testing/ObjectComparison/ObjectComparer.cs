// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlackBox.Testing;

namespace Microsoft.Test.ObjectComparison
{
    /// <summary>
    /// Represents a generic object comparer. This class uses an
    /// <seealso cref="ObjectGraphFactory"/> instance to convert objects to graph
    /// representations before comparing the representations.
    /// </summary>
    /// <remarks>
    /// Comparing two objects for equivalence is a relatively common task during test validation.
    /// One example would be to test whether a type follows the rules required by a particular
    /// serializer by saving and loading the object and comparing the two. A deep object
    /// comparison is one where all the properties and its properties are compared repeatedly
    /// until primitives are reached. The .NET Framework provides mechanisms to perform such comparisons but
    /// requires the types in question to implement part of the comparison logic
    /// (IComparable, .Equals). However, there are often types that do not follow
    /// these mechanisms. This API provides a mechanism to deep compare two objects using
    /// reflection.
    /// </remarks>
    ///
    /// <example>
    /// <p>The following example demonstrates how to compare two objects using a general-purpose object
    /// comparison strategy (represented by <see cref="PublicPropertyObjectGraphFactory"/>).</p>
    /// <code>
    /// // create an ObjectGraph factory
    /// ObjectGraphFactory factory = new PublicPropertyObjectGraphFactory();
    ///
    /// // instantiate the reusable comparer by passing the factory
    /// ObjectComparer comparer = new ObjectComparer(factory);
    ///
    /// // perform the compare operation
    /// bool match = comparer.Compare(obj1, obj2);
    /// if (!match)
    /// {
    /// Console.WriteLine("The two objects do not match.");
    /// }
    /// </code>
    /// </example>
    ///
    /// <example>
    /// <p>In addition, the object comparison API allows the user to get back a number of comparison mismatches
    /// (in the form of <see cref="ObjectComparisonMismatch"/> objects). The following example demonstrates
    /// how to do that.</p>
    /// <code>
    /// // create a list to hold the mismatches
    /// IEnumerable&lt;ObjectComparisonMismatch&gt; mismatches = new List&lt;ObjectComparisonMismatch&gt;();
    ///
    /// // if the objects don't match, print out the mismatches
    /// bool match = comparer.Compare(obj1, obj2, out mismatches);
    /// if (!match)
    /// {
    /// foreach (ObjectComparisonMismatch mismatch in mismatches)
    /// {
    /// Console.WriteLine(
    /// String.Format(
    /// "Nodes '{0}' and '{1}' do not match. Mismatch message: '{3}'",
    /// mismatch.LeftObjectNode.Name,
    /// mismatch.RightObjectNode.Name,
    /// mismatch.Description));
    /// }
    /// }
    /// </code>
    /// </example>
    public sealed class ObjectComparer
    {
        public ObjectGraphFactory ObjectGraphFactory { get; private set; }

        private readonly List<MemberInfo> _typePropertiesToIgnore;
        private readonly Dictionary<object, List<MemberInfo>> _instancePropertiesToIgnore;
        private readonly List<PropertyComparator> _customTypePropertyComparisons;
        private readonly Dictionary<object, List<PropertyComparator>> _customInstancePropertyComparisons;

        public ObjectComparer(ObjectGraphFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            ObjectGraphFactory = factory;
            _typePropertiesToIgnore = new List<MemberInfo>();
            _instancePropertiesToIgnore = new Dictionary<object, List<MemberInfo>>();
            _customTypePropertyComparisons = new List<PropertyComparator>();
            _customInstancePropertyComparisons = new Dictionary<object, List<PropertyComparator>>();
        }

        public void Ignore(object instance, MemberInfo propertyToIgnore)
        {
            if(instance == null)
                throw new ArgumentNullException("instance");

            if (!_instancePropertiesToIgnore.ContainsKey(instance))
                _instancePropertiesToIgnore.Add(instance, new List<MemberInfo>());
            _instancePropertiesToIgnore[instance].Add(propertyToIgnore);
        }

        public void IgnoreOnType(MemberInfo propertyToIgnore)
        {
            _typePropertiesToIgnore.Add(propertyToIgnore);
        }

        public void Allow(object instance, PropertyComparator customComparator)
        {
            if (!_customInstancePropertyComparisons.ContainsKey(instance))
                _customInstancePropertyComparisons.Add(instance, new List<PropertyComparator>());
            _customInstancePropertyComparisons[instance].Add(customComparator);
        }

        public void AllowOnType(PropertyComparator customComparator)
        {
            _customTypePropertyComparisons.Add(customComparator);
        }

        public IEnumerable<ObjectComparisonMismatch> Compare(object leftValue, object rightValue)
        {
            return CompareObjects(leftValue, rightValue);
        }

        private IEnumerable<ObjectComparisonMismatch> CompareObjects(object leftObject, object rightObject)
        {
            var mismatches = new List<ObjectComparisonMismatch>();

            GraphNode leftRoot = ObjectGraphFactory.CreateObjectGraph(leftObject);
            GraphNode rightRoot = ObjectGraphFactory.CreateObjectGraph(rightObject);

            var leftNodes = new List<GraphNode>(leftRoot.GetNodesInDepthFirstOrder());
            var rightNodes = new List<GraphNode>(rightRoot.GetNodesInDepthFirstOrder());

            FlagNodesThatShouldBeIgnored(leftNodes);

            // For each node in the left tree, search for the
            // node in the right tree and compare them
            foreach (GraphNode leftNode in leftNodes)
            {
                if(leftNode.Ignore)
                    continue;

                GraphNode rightNode = rightNodes.Where(node => leftNode.QualifiedName == node.QualifiedName)
                                                .DefaultIfEmpty(null)
                                                .FirstOrDefault();

                ObjectComparisonMismatch mismatch = CompareNodes(leftNode, rightNode);
                if(mismatch != null)
                    mismatches.Add(mismatch);
            }
            return mismatches;

        }

        private void FlagNodesThatShouldBeIgnored(IEnumerable<GraphNode> nodes)
        {
            foreach (var node in nodes)
            {
                if(node.Ignore)
                    continue;

                List<MemberInfo> propertiesToIgnore = FindPropertiesToIgnore(node);
                propertiesToIgnore.ForEach(node.IgnoreChild);
            }
        }

        private List<MemberInfo> FindPropertiesToIgnore(GraphNode node)
        {
            var propertiesToIgnore = new List<MemberInfo>();
            IEnumerable<MemberInfo> properties =
                node.ObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            propertiesToIgnore.AddRange(properties.Intersect(_typePropertiesToIgnore));
            if (node.ObjectValue != null && _instancePropertiesToIgnore.ContainsKey(node.ObjectValue))
            {
                IEnumerable<MemberInfo> instanceProperties = _instancePropertiesToIgnore[node.ObjectValue];
                propertiesToIgnore.AddRange(properties.Intersect(instanceProperties));
            }
            return propertiesToIgnore;
        }

        private ObjectComparisonMismatch CompareNodes(GraphNode leftNode, GraphNode rightNode)
        {
            if (IsAllowed(leftNode, rightNode))
                return null;

            // Check if both are null
            if (leftNode.ObjectValue == null && rightNode.ObjectValue == null)
                return null;

            // Check if left is null and right is not
            if (leftNode.ObjectValue == null)
                return new ObjectComparisonMismatch(leftNode,
                                                    rightNode,
                                                    ObjectComparisonMismatchType.MissingLeftNode);

            // Check if left is null and right is not
            if (rightNode.ObjectValue == null)
                return new ObjectComparisonMismatch(leftNode,
                                                    rightNode,
                                                    ObjectComparisonMismatchType.MissingRightNode);

            // Compare type names //
            if (!leftNode.ObjectType.Equals(rightNode.ObjectType))
                return new ObjectComparisonMismatch(leftNode,
                                                    rightNode,
                                                    ObjectComparisonMismatchType.ObjectTypesDoNotMatch);

            // Compare primitives, strings
            if (leftNode.ObjectType.IsPrimitive || leftNode.ObjectType.IsValueType || leftNode.ObjectType == typeof(string))
            {
                if (!leftNode.ObjectValue.Equals(rightNode.ObjectValue))
                    return new ObjectComparisonMismatch(leftNode,
                                                        rightNode,
                                                        ObjectComparisonMismatchType.ObjectValuesDoNotMatch);
                return null;
            }

            // Compare the child count
            if (leftNode.Children.Count != rightNode.Children.Count)
            {
                var mismatchType = leftNode.Children.Count > rightNode.Children.Count ?
                                   ObjectComparisonMismatchType.RightNodeHasFewerChildren :
                                   ObjectComparisonMismatchType.LeftNodeHasFewerChildren;

                return new ObjectComparisonMismatch(leftNode,
                                                    rightNode,
                                                    mismatchType);
            }

            return null;
        }

        private bool IsAllowed(GraphNode leftNode, GraphNode rightNode)
        {
            if (IsAllowed(_customTypePropertyComparisons, leftNode, rightNode))
                return true;

            if(rightNode.ObjectValue != null && 
               rightNode.Parent != null && 
               _customInstancePropertyComparisons.ContainsKey(rightNode.Parent.ObjectValue))
            {
                List<PropertyComparator> comparators = _customInstancePropertyComparisons[rightNode.Parent.ObjectValue];
                if (IsAllowed(comparators, leftNode, rightNode))
                    return true;
            }
            return false;
        }

        private bool IsAllowed(IEnumerable<PropertyComparator> comparators, GraphNode leftNode, GraphNode rightNode)
        {
            if (comparators.Select(c => c.Property).Contains(rightNode.Property))
            {
                PropertyComparator comparator =
                    comparators.First(c => c.Property == rightNode.Property);
                return ((bool)comparator.Comparator.DynamicInvoke(leftNode.ObjectValue, rightNode.ObjectValue));
            }
            return false;
        }
    }
}