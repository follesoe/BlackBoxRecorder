// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        #region Constuctors

        /// <summary>
        /// Creates an instance of the ObjectComparer class.
        /// </summary>
        /// <param name="factory">An ObjectGraphFactory to use for
        /// converting objects to graphs.</param>
        public ObjectComparer(ObjectGraphFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.objectGraphFactory = factory;
        }

        #endregion

        #region Public and Protected Members

        /// <summary>
        /// Gets the ObjectGraphFactory used to convert objects
        /// to graphs.
        /// </summary>
        public ObjectGraphFactory ObjectGraphFactory
        {
            get
            {
                return this.objectGraphFactory;
            }
        }

        public bool Compare(object leftValue,
                            object rightValue,
                            IEnumerable<MemberInfo> typePropertiesToIgnore,
                            Dictionary<object, List<MemberInfo>> instancePropertiesToIgnore,
                            IEnumerable<PropertyComparator> customTypePropertyComparisons,
                            Dictionary<object, List<PropertyComparator>> customInstancePropertyComparisons,
                            out IEnumerable<ObjectComparisonMismatch> mismatches)
        {
            if (typePropertiesToIgnore == null)
                typePropertiesToIgnore = new List<MemberInfo>();

            if (instancePropertiesToIgnore == null)
                instancePropertiesToIgnore = new Dictionary<object, List<MemberInfo>>();

            if (customTypePropertyComparisons == null)
                customTypePropertyComparisons = new List<PropertyComparator>();

            if(customInstancePropertyComparisons == null)
                customInstancePropertyComparisons = new Dictionary<object, List<PropertyComparator>>();

            List<ObjectComparisonMismatch> mismatch;
            bool isMatch = this.CompareObjects(leftValue,
                                               rightValue,
                                               typePropertiesToIgnore,
                                               instancePropertiesToIgnore,
                                               customTypePropertyComparisons,
                                               customInstancePropertyComparisons,
                                               out mismatch);
            mismatches = mismatch;
            return isMatch;
        }

        #endregion

        #region Private Members

        private bool CompareObjects(object leftObject,
                                    object rightObject,
                                    IEnumerable<MemberInfo> typePropertiesToIgnore,
                                    Dictionary<object, List<MemberInfo>> instancePropertiesToIgnore,
                                    IEnumerable<PropertyComparator> customTypePropertyComparisons,
                                    Dictionary<object, List<PropertyComparator>> customInstancePropertyComparisons,
                                    out List<ObjectComparisonMismatch> mismatches)
        {
            mismatches = new List<ObjectComparisonMismatch>();

            // Get the graph from the objects
            GraphNode leftRoot = this.ObjectGraphFactory.CreateObjectGraph(leftObject,
                                                                           typePropertiesToIgnore,
                                                                           instancePropertiesToIgnore,
                                                                           customTypePropertyComparisons,
                                                                           customInstancePropertyComparisons);
            GraphNode rightRoot = this.ObjectGraphFactory.CreateObjectGraph(rightObject,
                                                                            typePropertiesToIgnore,
                                                                            instancePropertiesToIgnore,
                                                                            customTypePropertyComparisons,
                                                                            customInstancePropertyComparisons);

            // Get the nodes in breadth first order
            List<GraphNode> leftNodes = new List<GraphNode>(leftRoot.GetNodesInDepthFirstOrder());
            List<GraphNode> rightNodes = new List<GraphNode>(rightRoot.GetNodesInDepthFirstOrder());

            // For each node in the left tree, search for the
            // node in the right tree and compare them
            for (int i = 0; i < leftNodes.Count; i++)
            {
                GraphNode leftNode = leftNodes[i];

                var nodelist = from node in rightNodes
                               where leftNode.QualifiedName.Equals(node.QualifiedName)
                               select node;

                List<GraphNode> matchingNodes = nodelist.ToList<GraphNode>();
                if (RightNodeIsMissingAndShouldNotBeIgnored(matchingNodes, leftNode))
                {
                    ObjectComparisonMismatch mismatch = new ObjectComparisonMismatch(leftNode, null, ObjectComparisonMismatchType.MissingRightNode);
                    mismatches.Add(mismatch);
                    continue;
                }

                GraphNode rightNode = matchingNodes[0];

                // Compare the nodes
                ObjectComparisonMismatch nodesMismatch = CompareNodes(leftNode, rightNode);
                if (ThereIsAMismatchWhichShouldNotBeIgnored(nodesMismatch, leftNode, rightNode))
                {
                    mismatches.Add(nodesMismatch);
                }
            }

            bool passed = mismatches.Count == 0 ? true : false;

            return passed;
        }

        private bool RightNodeIsMissingAndShouldNotBeIgnored(List<GraphNode> graphNodes, GraphNode leftNode)
        {
            return !(graphNodes.Any() || leftNode.Ignore);
        }

        private bool ThereIsAMismatchWhichShouldNotBeIgnored(ObjectComparisonMismatch potentialMismatch, GraphNode leftNode, GraphNode rightNode)
        {
            return potentialMismatch != null && !(leftNode.Ignore || rightNode.WithinAllowedRange);
        }

        private ObjectComparisonMismatch CompareNodes(GraphNode leftNode, GraphNode rightNode)
        {
            // Check if both are null
            if (leftNode.ObjectValue == null && rightNode.ObjectValue == null)
            {
                return null;
            }

            if (rightNode.WithinAllowedRange)
                return null;

            // check if one of them is null
            if (leftNode.ObjectValue == null || rightNode.ObjectValue == null)
            {
                ObjectComparisonMismatch mismatch = new ObjectComparisonMismatch(
                    leftNode,
                    rightNode,
                    ObjectComparisonMismatchType.ObjectValuesDoNotMatch);
                return mismatch;
            }

            // compare type names //
            if (!leftNode.ObjectType.Equals(rightNode.ObjectType))
            {
                ObjectComparisonMismatch mismatch = new ObjectComparisonMismatch(
                    leftNode,
                    rightNode,
                    ObjectComparisonMismatchType.ObjectTypesDoNotMatch);
                return mismatch;
            }

            // compare primitives, strings
            if (leftNode.ObjectType.IsPrimitive || leftNode.ObjectType.IsValueType || leftNode.ObjectType == typeof(string))
            {
                if (!leftNode.ObjectValue.Equals(rightNode.ObjectValue))
                {
                    ObjectComparisonMismatch mismatch = new ObjectComparisonMismatch(
                        leftNode,
                        rightNode,
                        ObjectComparisonMismatchType.ObjectValuesDoNotMatch);
                    return mismatch;
                }
                else
                {
                    return null;
                }
            }

            // compare the child count
            if (leftNode.Children.Count != rightNode.Children.Count)
            {
                var type = leftNode.Children.Count > rightNode.Children.Count ?
                    ObjectComparisonMismatchType.RightNodeHasFewerChildren : ObjectComparisonMismatchType.LeftNodeHasFewerChildren;

                ObjectComparisonMismatch mismatch = new ObjectComparisonMismatch(
                    leftNode,
                    rightNode,
                    type);
                return mismatch;
            }

            // No mismatch //
            return null;
        }

        #endregion

        #region Private Data

        private ObjectGraphFactory objectGraphFactory;

        #endregion
    }
}