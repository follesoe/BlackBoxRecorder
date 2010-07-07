using System;
using System.Runtime.Serialization;

namespace Threading
{
    /// <summary>
    /// Exception thrown by the <see cref="DeadlockMonitor"/> class when a deadlock
    /// is detected.
    /// </summary>
    [Serializable]
    public class DeadlockException : Exception
    {
        /// <summary>
        /// Initializes a new <see cref="DeadlockException"/>.
        /// </summary>
        public DeadlockException()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="DeadlockException"/> and specifies the message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public DeadlockException( string message ) : base( message )
        {
        }

        /// <summary>
        /// Initializes a new <see cref="DeadlockException"/> and specifies the message
        /// and inner <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner <see cref="Exception"/>.</param>
        public DeadlockException( string message, Exception inner ) : base( message, inner )
        {
        }

        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected DeadlockException(
            SerializationInfo info,
            StreamingContext context ) : base( info, context )
        {
        }
    }
}