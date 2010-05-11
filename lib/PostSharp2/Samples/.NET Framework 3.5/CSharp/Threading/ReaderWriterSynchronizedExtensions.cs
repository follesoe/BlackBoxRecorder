namespace Threading
{
    /// <summary>
    /// Utility methods to work with classes implementing <see cref="IReaderWriterSynchronized"/>.
    /// </summary>
    public static class ReaderWriterSynchronizedExtensions
    {
        /// <summary>
        /// Acquires an observer lock.
        /// </summary>
        /// <param name="obj">Synchronized object.</param>
        /// <returns>An object (considered opaque by client code) that should be disposed
        /// once the lock is not needed.</returns>
        /// <remarks>It is strongly recommended to use the <b>using</b> construct with
        /// this method.</remarks>
        public static ReaderWriterLockWrapper.Cookie AcquireObserverLock( this IReaderWriterSynchronized obj )
        {
            return obj.Lock.AcquireObserverLock();
        }

        /// <summary>
        /// Acquires a writer lock.
        /// </summary>
        /// <param name="obj">Synchronized object.</param>
        /// <returns>An object (considered opaque by client code) that should be disposed
        /// once the lock is not needed.</returns>
        /// <remarks>It is strongly recommended to use the <b>using</b> construct with
        /// this method.</remarks>
        public static ReaderWriterLockWrapper.Cookie AcquireWriteLock( this IReaderWriterSynchronized obj )
        {
            return obj.Lock.AcquireWriteLock();
        }

        /// <summary>
        /// Acquires a reader lock.
        /// </summary>
        /// <param name="obj">Synchronized object.</param>
        /// <returns>An object (considered opaque by client code) that should be disposed
        /// once the lock is not needed.</returns>
        /// <remarks>It is strongly recommended to use the <b>using</b> construct with
        /// this method.</remarks>
        public static ReaderWriterLockWrapper.Cookie AcquireReadLock( this IReaderWriterSynchronized obj )
        {
            return obj.Lock.AcquireReadLock();
        }

        /// <summary>
        /// Acquires an upgradable reader lock.
        /// </summary>
        /// <param name="obj">Synchronized object.</param>
        /// <returns>An object (considered opaque by client code) that should be disposed
        /// once the lock is not needed.</returns>
        /// <remarks>It is strongly recommended to use the <b>using</b> construct with
        /// this method.</remarks>
        public static ReaderWriterLockWrapper.Cookie AcquireUpgradableReadLock( this IReaderWriterSynchronized obj )
        {
            return obj.Lock.AcquireUpgradableReadLock();
        }
    }
}