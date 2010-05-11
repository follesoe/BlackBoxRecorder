using System;
using System.Runtime.Serialization;

namespace Threading
{
    /// <summary>
    /// Exception thrown when a lock has been acquired but has not been disposed.
    /// This exception is typically thrown from the Garbage Collection thread.
    /// </summary>
    [Serializable]
    public class LockNotDisposedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        /// <summary>
        /// Initializes a new <see cref="LockNotDisposedException"/>.
        /// </summary>
        public LockNotDisposedException()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="LockNotDisposedException"/> with an exception message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public LockNotDisposedException( string message ) : base( message )
        {
        }


        /// <summary>
        /// Initializes a new <see cref="LockNotDisposedException"/> with an exception message
        /// and an inner <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public LockNotDisposedException( string message, Exception inner ) : base( message, inner )
        {
        }

        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected LockNotDisposedException(
            SerializationInfo info,
            StreamingContext context ) : base( info, context )
        {
        }
    }
}