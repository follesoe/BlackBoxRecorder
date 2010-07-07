using System.Collections.Generic;

namespace Threading
{
    /// <summary>
    /// Implementation of some graph algorithms (the breadth-first search algorithm, specifically).
    /// </summary>
    internal sealed class GraphUtil
    {
        public const int Unreachable = int.MaxValue;
        public const int Cycle = int.MinValue;

        private readonly IGraph graph;

        /// <summary>
        /// Initializes a new <see cref="GraphUtil"/>.
        /// </summary>
        /// <param name="graph">The graph on which the algorithms should work.</param>
        public GraphUtil( IGraph graph )
        {
            this.graph = graph;
        }


        /// <summary>
        /// Gets the initial vector to be passed to <see cref="DoBreadthFirstSearch"/>.
        /// </summary>
        /// <returns>The initial vector to be passed to <see cref="DoBreadthFirstSearch"/>
        /// (a vector whose each element is set to <see cref="Unreachable"/>.</returns>
        public int[] GetInitialVector()
        {
            int n = (byte) this.graph.NodeCount;
            int[] vector = new int[n];
            for ( int i = 0; i < n; i++ )
            {
                vector[i] = Unreachable;
            }

            return vector;
        }

        /// <summary>
        /// Perform a breadth-first search on the graph associated with the current instance,
        /// i.e. finds the shortest path and distance between a given initial node and other nodes
        /// of the graph.
        /// </summary>
        /// <param name="initialNode">The initial node.</param>
        /// <param name="distances">At output, this vector is filled with the distance of each node and the initial node, 
        /// When a cycle is detected, one element of the cycle is marked by the <see cref="Cycle"/> constant in this vector.
        /// Vector elements representing nodes that cannot be reached from <paramref name="initialNode"/> are not updated.
        /// </param>
        /// <param name="directPredecessors">At output, this vector is filled with the direct predecessor of the corresponding node.</param>
        /// <remarks>
        /// <para>Before <see cref="DoBreadthFirstSearch"/> is invoked, the vectors <paramref name="distances"/> and <paramref name="directPredecessors"/>
        /// should be initialized using the <see cref="GetInitialVector"/> method. This method returns a vector where all elements are initialized
        /// to <see cref="Unreachable"/>.</para>
        /// <para>The <see cref="DoBreadthFirstSearch"/> method can be invoked many times consecutively (typically, if all nodes are not connected)
        /// with the same vectors. A call of <see cref="DoBreadthFirstSearch"/> does not consider nodes that have been reached by a previous
        /// invocation of the method. Cycles are indicated by a <see cref="Cycle"/> value in the <paramref name="distances"/> vector.
        /// In this case, it is possible to discover all nodes involved in the cycle by using the <paramref name="directPredecessors"/> vector.</para>
        /// </remarks>
        public void DoBreadthFirstSearch( int initialNode, int[] distances, int[] directPredecessors )
        {
            int n = this.graph.NodeCount;


            distances[initialNode] = 0;

            Queue<int> queue = new Queue<int>( n );
            queue.Enqueue( initialNode );

            while ( queue.Count > 0 )
            {
                int current = queue.Dequeue();
                int currentDistance = distances[current];

                foreach ( int successor in this.graph.GetSuccessors( current ) )
                {
                    int successorDistance = distances[successor];
                    if ( successorDistance == Unreachable )
                    {
                        distances[successor] = (sbyte) (currentDistance + 1);
                        directPredecessors[successor] = current;

                        queue.Enqueue( successor );
                    }
                    else if ( successorDistance == Cycle )
                    {
                        // We have already discovered that the successor is part of a cycle.
                    }
                    else
                    {
                        // Trace back all predecessors to see if the successor is not already a predecessor.
                        int cursor = current;
                        while ( cursor != initialNode && distances[cursor] != Cycle )
                        {
                            if ( cursor == successor )
                            {
                                // We just discovered that the successor is part of a cycle.
                                distances[successor] = Cycle;
                                directPredecessors[successor] = current;
                                break;
                            }
                            cursor = directPredecessors[cursor];
                        }
                    }
                }
            }
        }
    }
}