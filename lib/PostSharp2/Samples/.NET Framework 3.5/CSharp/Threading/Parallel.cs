using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Threading
{
    /// <summary>
    /// Utility methods to make code execute in parallel.
    /// </summary>
    public static class Parallel
    {
        /// <summary>
        /// Executes a action for each element of a weakly typed enumerable.
        /// </summary>
        /// <param name="enumerable">An enumerable.</param>
        /// <param name="action">The action to be executed.</param>
        public static void ForEach( IEnumerable enumerable, Action<object> action )
        {
            if ( enumerable == null ) throw new ArgumentNullException( "enumerable" );
            if ( action == null ) throw new ArgumentNullException( "action" );

            foreach ( object item in enumerable )
            {
                ThreadPool.QueueUserWorkItem( state => action( state ), item );
            }
        }

        /// <summary>
        /// Executes an action for each element of a strongly typed enumerable
        /// </summary>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <param name="enumerable">An enumerable.</param>
        /// <param name="action">The action to be executed.</param>
        public static void ForEach<T>( IEnumerable<T> enumerable, Action<T> action )
        {
            if ( enumerable == null ) throw new ArgumentNullException( "enumerable" );
            if ( action == null ) throw new ArgumentNullException( "action" );

            foreach ( T item in enumerable )
            {
                ThreadPool.QueueUserWorkItem( state => action( (T) state ), item );
            }
        }

        /// <summary>
        /// Executes an action for each element of an enumeration, and wait until all
        /// actions have executed.
        /// </summary>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <param name="enumerable">An enumerable.</param>
        /// <param name="action">The action to be executed.</param>
        public static void ForEachWaiting<T>( IEnumerable<T> enumerable, Action<T> action )
        {
            Job<T> job = new Job<T>( action );

            foreach ( T item in enumerable )
            {
                job.Enqueue( item );
            }

            job.DoneEvent.WaitOne();
        }

        private class Job<T>
        {
            private readonly Action<T> action;
            private int numberPending;
            private readonly ManualResetEvent doneEvent = new ManualResetEvent( true );

            public ManualResetEvent DoneEvent
            {
                get { return this.doneEvent; }
            }

            public Job( Action<T> action )
            {
                this.action = action;
            }

            public void Enqueue( object state )
            {
                doneEvent.Reset();
                Interlocked.Increment( ref this.numberPending );
                ThreadPool.QueueUserWorkItem( this.Do, state );
            }

            private void Do( object state )
            {
                try
                {
                    action( (T) state );
                }
                finally
                {
                    int remainingThreads = Interlocked.Decrement( ref this.numberPending );

                    if ( remainingThreads == 0 )
                    {
                        this.doneEvent.Set();
                    }
                }
            }
        }
    }
}