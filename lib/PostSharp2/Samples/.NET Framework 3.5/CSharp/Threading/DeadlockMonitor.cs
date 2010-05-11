using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using ThreadState=System.Threading.ThreadState;

namespace Threading
{
    /// <summary>
    /// Detects deadlocks occurring because of circular wait conditions.
    /// </summary>
    /// <remarks>
    /// 	<para>
    ///         The <see cref="DeadlockMonitor"/> works by building, in real time, a graph
    ///         of dependencies between threads and waiting objects. Waiting objects have to
    ///         cooperate with the <see cref="DeadlockMonitor"/> to add and remove edges
    ///         to the graph. Currently, the only synchronization objects cooperating with
    ///         <see cref="DeadlockMonitor"/> are <see cref="ReaderWriterLockWrapper"/> and 
    ///         <b>WpfDispatchAttribute</b>. Therefore,
    ///         deadlocks occurring because of other objects will not be detected.
    ///     </para>
    /// 	<para>
    ///         Synchronization objects update the wait graph by calling methods <see cref="EnterWaiting"/> 
    ///         (before starting to wait), <see cref="ConvertWaitingToAcquired"/> (when a synchronization object has been
    ///         acquired after waiting), <see cref="EnterAcquired"/> (when a
    ///         synchronization object has been acquired without waiting), <see cref="ExitWaiting"/> 
    ///         (after waiting, when the synchronization object has not
    ///         been acquired), or <see cref="ExitAcquired"/> (after the synchronization
    ///         object has been released).
    ///     </para>
    /// 	<para>
    ///         Synchronization objects are expected to wait only a defined amount of time.
    ///         When this amount of time has elapsed, they should cause a deadlock detection
    ///         by invoking the <see cref="DetectDeadlocks"/> methods. This method will
    ///         analyze the dependency graph for cycles and throw a <see cref="DeadlockException"/> if 
    ///         a deadlock is detected. Additionally, all threads
    ///         that participate in the deadlock will be interrupted using <see cref="Thread.Interrupt"/>.
    ///     </para>
    /// </remarks>
    /// <notes>
    ///     Synchronization objects can have locks of many roles. For instance, a 
    ///     <see cref="ReaderWriterLockSlim"/> object has to be represented with three roles:
    ///     <em>read</em>, <em>write</em> and <em>upgradeable read</em>. Individual roles of
    ///     synchronization objects are considered as separate nodes in the dependency graph.
    /// </notes>
    public static class DeadlockMonitor
    {
        private static readonly Dictionary<Edge, EdgeInfo> edges = new Dictionary<Edge, EdgeInfo>();
        private static int revision;
        private static bool detectionPending;
        private static readonly Random random = new Random();

        private static void RandomSleep()
        {
            int r;
            lock ( random )
            {
                r = random.Next( 100 );
            }

            if ( r < 10 )
                Thread.Sleep( r );
        }

        private static void AddEdge( object predecessor, string predecessorRole, object predecessorInfo, object successor, string successorRole,
                                     object successorInfo )
        {
            EdgeInfo edgeInfo;


            revision++;

            Edge edge = new Edge( predecessor, predecessorRole, successor, successorRole );
            if ( !edges.TryGetValue( edge, out edgeInfo ) )
            {
                edgeInfo = new EdgeInfo {Edge = edge, Counter = 1};
                edges.Add( edge, edgeInfo );
            }
            else
            {
                edgeInfo.Counter++;
            }


            edgeInfo.LastChange = Environment.TickCount;
            if ( predecessorInfo != null ) edgeInfo.PredecessorInfo = predecessorInfo;
            if ( successorInfo != null ) edgeInfo.SuccessorInfo = successorInfo;
        }

        private static void RemoveEdge( object predecessor, string predecessorRole, object successor, string successorRole )
        {
            revision++;

            Edge edge = new Edge( predecessor, predecessorRole, successor, successorRole );
            EdgeInfo edgeInfo;
            if ( !edges.TryGetValue( edge, out edgeInfo ) )
            {
                throw new InvalidOperationException( string.Format( "Cannot remove edge {{{0}}} because it has not beed registered before.",
                                                                    edge.ToString() ) );
            }
            else
            {
                edgeInfo.Counter--;
                edgeInfo.LastChange = Environment.TickCount;

                if ( edgeInfo.Counter == 0 )
                    edges.Remove( edge );
            }
        }


        /// <summary>Method to be invoked before starting to wait for a synchronization object.</summary>
        /// <param name="syncObject">The synchronization object that will be waited for.</param>
        /// <param name="syncObjectRole">
        ///     Role (or name) of the lock, inside <paramref name="syncObject"/>, or
        ///     <strong>null</strong> if the synchronization object has no role.
        /// </param>
        /// <param name="syncObjectInfo">
        ///     Object representing <paramref name="syncObject"/> when human-readable information
        ///     is displayed (the <see cref="object.ToString"/> method of this object is used), or
        ///     <strong>null</strong>
        /// </param>
        [Conditional( "DEBUG" )]
        public static void EnterWaiting( object syncObject, string syncObjectRole, object syncObjectInfo )
        {
            lock ( edges )
            {
                AddEdge( Thread.CurrentThread, null, null, syncObject, syncObjectRole, syncObjectInfo );
            }
        }

        /// <summary>Method to be invoked after waiting for a synchronization object, typically when the object
        /// has not been acquired.</summary>
        /// <param name="syncObject">The synchronization object that is no longer waited for.</param>
        /// <param name="syncObjectRole">
        ///     Role (or name) of the lock, inside <paramref name="syncObject"/>, or
        ///     <strong>null</strong> if the synchronization object has no role.
        /// </param>
        [Conditional( "DEBUG" )]
        public static void ExitWaiting( object syncObject, string syncObjectRole )
        {
            lock ( edges )
            {
                RemoveEdge( Thread.CurrentThread, "", syncObject, syncObjectRole );
            }
        }

        /// <summary>Method to be invoked after waiting for a synchronization object,
        /// when it has been acquired.</summary>
        /// <param name="syncObject">The synchronization object that has been acquired.</param>
        /// <param name="syncObjectRole">
        ///     Role (or name) of the lock, inside <paramref name="syncObject"/>, or
        ///     <strong>null</strong> if the synchronization object has no role.
        /// </param>
        /// <param name="syncObjectInfo">
        ///     Object representing <paramref name="syncObject"/> when human-readable information
        ///     is displayed (the <see cref="object.ToString"/> method of this object is used), or
        ///     <strong>null</strong>
        /// </param>
        [Conditional( "DEBUG" )]
        public static void ConvertWaitingToAcquired( object syncObject, string syncObjectRole, object syncObjectInfo )
        {
            Thread thread = Thread.CurrentThread;
            lock ( edges )
            {
                RemoveEdge( thread, "", syncObject, syncObjectRole );
                AddEdge( syncObject, syncObjectRole, syncObjectInfo, thread, "", null );
            }

            RandomSleep();
        }

        /// <summary>Method to be invoked after a synchronization object has been acquired, typically
        /// when the object has not been waited for (in this case, methods <see cref="EnterWaiting"/>
        /// and <see cref="ConvertWaitingToAcquired"/> would have been called).</summary>
        /// <param name="syncObject">The synchronization object that has been acquired.</param>
        /// <param name="syncObjectRole">
        ///     Role (or name) of the lock, inside <paramref name="syncObject"/>, or
        ///     <strong>null</strong> if the synchronization object has no role.
        /// </param>
        /// <param name="syncObjectInfo">
        ///     Object representing <paramref name="syncObject"/> when human-readable information
        ///     is displayed (the <see cref="object.ToString"/> method of this object is used), or
        ///     <strong>null</strong>
        /// </param>
        [Conditional( "DEBUG" )]
        public static void EnterAcquired( object syncObject, string syncObjectRole, object syncObjectInfo )
        {
            lock ( edges )
            {
                AddEdge( syncObject, syncObjectRole, syncObjectInfo, Thread.CurrentThread, "", null );
            }

            RandomSleep();
        }

        /// <summary>Method to be invoked after a synchronization object has been released.</summary>
        /// <param name="syncObject">The synchronization object that has been released.</param>
        /// <param name="syncObjectRole">
        ///     Role (or name) of the lock, inside <paramref name="syncObject"/>, or
        ///     <strong>null</strong> if the synchronization object has no role.
        /// </param>
        [Conditional( "DEBUG" )]
        public static void ExitAcquired( object syncObject, string syncObjectRole )
        {
            lock ( edges )
            {
                RemoveEdge( syncObject, syncObjectRole, Thread.CurrentThread, "" );
            }
        }

        /// <summary>
        /// Detects deadlocks by analyzing the graph of wait dependencies.
        /// </summary>
        /// <remarks>
        /// 	<para>The algorithm works by analyzing cycles in the dependency graph. Cycles
        ///     indicate potential deadlocks, and their may be situations where a cycle does not
        ///     correspond to a deadlock.</para>
        /// 	<para>
        ///         Indeed, and although the dependency graph is updated in real time, it may not
        ///         always correspond to the reality. For instance, a deadlock detection may occur
        ///         while some thread is not waiting, for instance after the invocation of a
        ///         <strong>Wait</strong> method and before the invocation of the 
        ///         <see cref="ConvertWaitingToAcquired"/> method.
        ///     </para>
        /// 	<para>To make sure that all threads are waiting, we do the following when we detect
        ///     a cycle in the dependency graph:</para>
        /// 	<list type="bullet">
        /// 		<item></item>
        /// 		<item>We require every edge involved in a cycle to be at least 50 ms
        ///         old.</item>
        /// 		<item>We require every thread involved in a cycle to be in waiting
        ///         state.</item>
        /// 	</list>
        /// 	<para class="xmldocbulletlist">If these conditions are not met, we restart the
        ///     detection (after sleeping some time, in the first case).</para>
        /// 	<para class="xmldocbulletlist">
        ///         When a cycle is detected and identified as a deadlock, we produce an exhaustive
        ///         report of the deadlock cycle, including the stack trace of all threads
        ///         involved. Then, we interrupt each thread invoked using the <see cref="Thread.Interrupt"/> method; 
        ///         we throw a <see cref="DeadlockException"/> in the current thread.
        ///     </para>
        /// 	<para class="xmldocbulletlist">When a deadlock is detected, the application is
        ///     expected to stop, since it may have been left in an inconsistent state.</para>
        /// </remarks>
        [Conditional( "DEBUG" )]
        public static void DetectDeadlocks()
        {
            if ( detectionPending )
            {
                Debug.Print( "Deadlock detection skipped because another one is pending." );
                return;
            }

            detectionPending = true;

            Debug.Print( "Deadlock detection started." );

            try
            {
                int iterations = 0;

                detectionLoop:
                iterations++;

                if ( iterations > 5 )
                {
                    Debug.Print( "Too many deadlocks detections. Maybe there is no deadlock, after all." );
                    return;
                }

                // Take a deep clone of the graph, so we can analyze without blocking other threads.
                // (It is essential that other threads are not blocking when a cycle is detected,
                //  because only blocked threads are considered to be deadlocked.)
                Dictionary<Edge, EdgeInfo> clonedEdges = new Dictionary<Edge, EdgeInfo>( edges.Count );
                lock ( edges )
                {
                    foreach ( KeyValuePair<Edge, EdgeInfo> pair in edges )
                    {
                        Edge edge = pair.Key;


                        clonedEdges.Add( edge, pair.Value.Clone() );
                        Debug.Print( "In graph: {0}.", pair.Value );
                    }

                    // If a thread waits for an object it already owns, ignore this wait.
                    foreach ( KeyValuePair<Edge, EdgeInfo> pair in edges )
                    {
                        Edge edge = pair.Key;

                        if ( edge.Successor.SyncObject is Thread &&
                             !(edge.Predecessor.SyncObject is Thread) )
                        {
                            clonedEdges.Remove( new Edge( edge.Successor, edge.Predecessor ) );
                        }
                    }
                }

                Dictionary<Node, int> nodeIndexes = new Dictionary<Node, int>();
                List<Node> nodes = new List<Node>();


                // Creates a list of node indexes.
                foreach ( KeyValuePair<Edge, EdgeInfo> edge in clonedEdges )
                {
                    if ( !nodeIndexes.ContainsKey( edge.Key.Predecessor ) )
                    {
                        nodeIndexes.Add( edge.Key.Predecessor, nodeIndexes.Count );
                        nodes.Add( edge.Key.Predecessor );
                    }

                    if ( !nodeIndexes.ContainsKey( edge.Key.Successor ) )
                    {
                        nodeIndexes.Add( edge.Key.Successor, nodeIndexes.Count );
                        nodes.Add( edge.Key.Successor );
                    }
                }

                // Create the list of successors.
                int n = nodes.Count;
                LinkedList<EdgeInfo>[] successors = new LinkedList<EdgeInfo>[n];
                bool[] hasPredecessor = new bool[n];
                foreach ( KeyValuePair<Edge, EdgeInfo> edge in clonedEdges )
                {
                    int predecessorIndex = nodeIndexes[edge.Key.Predecessor];
                    LinkedList<EdgeInfo> mySuccessors = successors[predecessorIndex];
                    if ( mySuccessors == null )
                    {
                        mySuccessors = new LinkedList<EdgeInfo>();
                        successors[predecessorIndex] = mySuccessors;
                    }
                    mySuccessors.AddLast( edge.Value );
                    hasPredecessor[nodeIndexes[edge.Key.Successor]] = true;
                }

                // Resolve the graph.
                Graph graph = new Graph( successors, nodeIndexes );
                GraphUtil graphUtil = new GraphUtil( graph );
                int[] distances = graphUtil.GetInitialVector();
                int[] predecessors = graphUtil.GetInitialVector();

                bool hasNodeWithoutPredecessor = false;
                for ( int i = 0; i < n; i++ )
                {
                    if ( !hasPredecessor[i] )
                    {
                        hasNodeWithoutPredecessor = true;
                        graphUtil.DoBreadthFirstSearch( i, distances, predecessors );
                    }
                }

                if ( !hasNodeWithoutPredecessor )
                    graphUtil.DoBreadthFirstSearch( 0, distances, predecessors );

                // Inspect cycles in the graph.
                for ( int i = 0; i < n; i++ )
                {
                    if ( distances[i] == GraphUtil.Cycle )
                    {
                        // We found a cycle. Analyze it to produce a meaningful error message.
                        Debug.Print( "Found a cycle in thread dependencies." );

                        StringBuilder messageBuilder = new StringBuilder( "Deadlock detected. The following synchronization elements form a cycle: " );

                        List<Thread> threadsInDeadlock = new List<Thread>();
                        int cursor = i;
                        int mostRecentEdgeChange = 0;
                        do
                        {
                            int successor = cursor;
                            int predecessor = predecessors[successor];

                            Edge edge = new Edge( nodes[predecessor], nodes[successor] );
                            EdgeInfo edgeInfo = clonedEdges[edge];

                            Debug.Print( "In cycle: {0}.", edgeInfo );

                            if ( cursor == i )
                            {
                                Thread successorThread = edge.Successor.SyncObject as Thread;

                                if ( successorThread != null )
                                {
                                    threadsInDeadlock.Add( successorThread );
                                }
                                else
                                {
                                }

                                messageBuilder.AppendFormat( "#{1}={{{0}}}", edgeInfo.Edge.Successor.Format( edgeInfo.SuccessorInfo ), successor );
                            }
                            messageBuilder.Append( " <- " );

                            Thread predecessorThread = edge.Predecessor.SyncObject as Thread;

                            if ( predecessorThread != null )
                            {
                                if ( !threadsInDeadlock.Contains( predecessorThread ) )
                                {
                                    threadsInDeadlock.Add( predecessorThread );
                                }
                            }

                            messageBuilder.AppendFormat( "#{1}={{{0}}}", edgeInfo.Edge.Predecessor.Format( edgeInfo.PredecessorInfo ), predecessor );

                            if ( mostRecentEdgeChange < edgeInfo.LastChange )
                                mostRecentEdgeChange = edgeInfo.LastChange;

                            cursor = predecessor;
                        } while ( cursor != i );

                        mostRecentEdgeChange = Environment.TickCount - mostRecentEdgeChange;
                        if ( mostRecentEdgeChange < 50 )
                        {
                            Debug.Print( "Most recent change to edges in cycle is {0} ms. Wait until all threads reach a stable point.",
                                         mostRecentEdgeChange );
                            Thread.Sleep( 50 - mostRecentEdgeChange );
                            goto detectionLoop;
                        }


                        messageBuilder.Append( "." );

                        foreach ( Thread thread in threadsInDeadlock )
                        {
                            if ( thread != Thread.CurrentThread )
                            {
                                messageBuilder.AppendFormat(
                                    Environment.NewLine +
                                    Environment.NewLine +
                                    "-- start of stack trace of thread {0} (Name=\"{1}\"):" + Environment.NewLine,
                                    thread.ManagedThreadId, thread.Name );

                                if ( thread.ThreadState != ThreadState.WaitSleepJoin )
                                {
                                    // We have captured a cycle, but not a deadlock since some thread is not in waiting state.
                                    // This may happen if DetectDeadlocks was invoked when another thread was just between
                                    // an EnterWaiting and ConvertWaitingToAcquired, and was waiting for a non-blocked lock.

                                    Debug.Print(
                                        "Deadlock detection aborted. Thread {0} (Name=\"{1}\") is in state {2} (instead of being waiting)." +
                                        Environment.NewLine,
                                        thread.ManagedThreadId, thread.Name, thread.ThreadState );
                                    return;
                                }

                                try
                                {
#pragma warning disable 612,618
                                    thread.Suspend();
                                    StackTrace stackTrace = new StackTrace( thread, true );
                                    messageBuilder.Append( stackTrace.ToString() );
                                    thread.Resume();
                                    thread.Interrupt();
#pragma warning restore 612,618
                                }
                                catch ( Exception e )
                                {
                                    messageBuilder.Append( "Cannot get a stack trace: " );
                                    messageBuilder.Append( e.Message );
                                }

                                messageBuilder.AppendFormat( Environment.NewLine + "-- end of stack trace of thread {0}", thread.ManagedThreadId );
                            }
                            else
                            {
                                messageBuilder.AppendFormat( Environment.NewLine + Environment.NewLine + "-- current thread is {0} (Name=\"{1}\")",
                                                             thread.ManagedThreadId, thread.Name );
                            }
                        }

                        throw new DeadlockException( messageBuilder.ToString() );
                    }
                }

                Debug.Print( "No cycle detected." );
            }
            finally
            {
                detectionPending = false;
            }
        }

        private struct Node : IEquatable<Node>
        {
            public readonly object SyncObject;
            private readonly string role;

            public Node( object syncObject, string role )
            {
                this.SyncObject = syncObject;
                this.role = role ?? "";
            }

            public bool Equals( Node other )
            {
                return ReferenceEquals( this.SyncObject, other.SyncObject ) && this.role == other.role;
            }

            public override bool Equals( object obj )
            {
                return this.Equals( (Node) obj );
            }

            public override int GetHashCode()
            {
                return (this.SyncObject.GetHashCode() << 16) | this.role.GetHashCode();
            }

            public override string ToString()
            {
                return Format( null );
            }

            public string Format( object objInfo )
            {
                Thread thread = this.SyncObject as Thread;
                if ( thread != null )
                {
                    return string.Format( "{{Thread {0}, Name=\"{1}\"}}", thread.ManagedThreadId, thread.Name );
                }
                else
                {
                    return string.Format( "{{{0}:{1}}}", objInfo ?? this.SyncObject, this.role );
                }
            }
        }


        private struct Edge : IEquatable<Edge>
        {
            public readonly Node Predecessor;
            public readonly Node Successor;

            public Edge( Node predecessor, Node successor )
            {
                this.Predecessor = predecessor;
                this.Successor = successor;
            }

            public Edge( object predecessor, string predecessorRole, object successor, string successorRole )
            {
                this.Predecessor = new Node( predecessor, predecessorRole );
                this.Successor = new Node( successor, successorRole );
            }


            public bool Equals( Edge other )
            {
                return this.Predecessor.Equals( other.Predecessor ) &&
                       this.Successor.Equals( other.Successor );
            }

            public override int GetHashCode()
            {
                return this.Predecessor.GetHashCode() | ~this.Successor.GetHashCode();
            }

            public override bool Equals( object obj )
            {
                return this.Equals( (Edge) obj );
            }

            public override string ToString()
            {
                return string.Format( "{{{0} -> {1}}}", this.Predecessor, this.Successor );
            }
        }

        private class EdgeInfo
        {
            public Edge Edge;
            public object PredecessorInfo;
            public object SuccessorInfo;
            public int Counter;
            public int LastChange;

            public EdgeInfo Clone()
            {
                return new EdgeInfo
                           {
                               Edge = Edge,
                               Counter = Counter,
                               LastChange = LastChange,
                               PredecessorInfo = PredecessorInfo,
                               SuccessorInfo = SuccessorInfo
                           };
            }

            public override string ToString()
            {
                return string.Format( "{{{0}}}->{{{1}}}, Counter={2}",
                                      this.Edge.Predecessor.Format( this.PredecessorInfo ),
                                      this.Edge.Successor.Format( this.SuccessorInfo ),
                                      this.Counter );
            }
        }

        private class Graph : IGraph
        {
            private readonly LinkedList<EdgeInfo>[] successors;
            private readonly Dictionary<Node, int> nodeIndexes;

            public Graph( LinkedList<EdgeInfo>[] successors, Dictionary<Node, int> nodeIndexes )
            {
                this.successors = successors;
                this.nodeIndexes = nodeIndexes;
            }

            #region Implementation of IGraph

            public int NodeCount
            {
                get { return this.successors.Length; }
            }

            public IEnumerable<int> GetSuccessors( int predecessor )
            {
                LinkedList<EdgeInfo> s = successors[predecessor];
                if ( s == null ) yield break;
                foreach ( EdgeInfo edgeInfo in s )
                {
                    yield return this.nodeIndexes[edgeInfo.Edge.Successor];
                }
            }

            #endregion
        }
    }
}