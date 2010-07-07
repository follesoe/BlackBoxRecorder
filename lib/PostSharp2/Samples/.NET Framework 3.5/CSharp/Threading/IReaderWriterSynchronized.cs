using System.Threading;

namespace Threading
{
    /// <summary>
    /// Interface to be implemented by classes whose instances are synchronized by
    /// a <see cref="ReaderWriterLockSlim"/>.
    /// </summary>
    public interface IReaderWriterSynchronized
    {
        /// <summary>
        /// Gets the <see cref="ReaderWriterLockWrapper"/> that has to be used
        /// to synchronize access to the current instance.
        /// </summary>
        ReaderWriterLockWrapper Lock { get; }
    }
}