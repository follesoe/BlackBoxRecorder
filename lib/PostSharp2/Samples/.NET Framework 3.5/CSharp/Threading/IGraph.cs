using System.Collections.Generic;

namespace Threading
{
    /// <summary>
    /// Semantics of a graph.
    /// </summary>
    internal interface IGraph
    {
        /// <summary>
        /// Gets the number of nodes of the current graph.
        /// </summary>
        int NodeCount { get; }

        /// <summary>
        /// Gets the successors of a given node.
        /// </summary>
        /// <param name="predecessor">Index of the predecessor.</param>
        /// <returns>An enumeration of the index of all successors of <paramref name="predecessor"/>.</returns>
        IEnumerable<int> GetSuccessors( int predecessor );
    }
}