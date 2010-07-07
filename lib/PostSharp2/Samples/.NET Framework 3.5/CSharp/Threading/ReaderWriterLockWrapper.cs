using System;
using System.Diagnostics;
using System.Threading;

namespace Threading
{
    /// <summary>
    /// Wraps the class <see cref="ReaderWriterLockSlim"/> in a safe way,
    /// and cooperates with <see cref="DeadlockMonitor"/>.
    /// </summary>
    public sealed class ReaderWriterLockWrapper
    {
        private readonly ReaderWriterLockSlim @lock;
        private readonly object lockedObject;

        internal const int WarningTimeout = 500;
        private const int DeadlockDetectionPeriod = 1000;
        private const int GarbageCollectionDetectionPeriod = 500;
        private const string readLock = "r";
        private const string upgradableReadLock = "ur";
        private const string writeLock = "w";

        /// <summary>
        /// An instance of <see cref="ReaderWriterLockWrapper"/> without underlying <see cref="ReaderWriterLockSlim"/>.
        /// All acquisition requests are accepted and no lock is actually acquired.
        /// </summary>
        public static readonly ReaderWriterLockWrapper NoLock = new ReaderWriterLockWrapper( null, null );

        private ReaderWriterLockWrapper( ReaderWriterLockSlim @lock, object lockedObject )
        {
            this.@lock = @lock;
            this.lockedObject = lockedObject;
        }

        /// <summary>
        /// Initialized a new <see cref="ReaderWriterLockWrapper"/>.
        /// </summary>
        /// <param name="lockedObject">Object to which the lock is primarily associated,
        /// used to display debugging information.</param>
        public ReaderWriterLockWrapper( object lockedObject )
            : this( new ReaderWriterLockSlim(), lockedObject )
        {
        }


        /// <summary>
        /// Determines whether the read lock is held.
        /// </summary>
        public bool IsReadLockHeld
        {
            get { return this.@lock == null || this.@lock.IsReadLockHeld || this.@lock.IsWriteLockHeld || this.@lock.IsUpgradeableReadLockHeld; }
        }


        private static void DetectProblems( int i )
        {
            if ( i%DeadlockDetectionPeriod/WarningTimeout == 0 )
            {
                DeadlockMonitor.DetectDeadlocks();
            }

            // Doing a GC will collect eventual ghost locks.
            if ( i%GarbageCollectionDetectionPeriod/WarningTimeout == 0 )
            {
                GC.Collect();
            }
        }

        private static StackTrace GetStackTrace()
        {
#if DEBUG
            return new StackTrace( 3 );
#else
            return null;
#endif
        }

        /// <summary>
        /// Acquires an observer lock.
        /// </summary>
        /// <returns>An object (considered opaque by client code) that should be disposed
        /// once the lock is not needed.</returns>
        /// <remarks>It is strongly recommended to use the <b>using</b> construct with
        /// this method.</remarks>
        public Cookie AcquireObserverLock()
        {
            if ( this.@lock == null ) return new Cookie( CookieAction.None, null, null );

            if ( this.@lock.IsWriteLockHeld )
            {
                this.@lock.ExitWriteLock();
                DeadlockMonitor.ExitAcquired( this.@lock, writeLock );
                return new Cookie( CookieAction.EnterWriteLock, this, GetStackTrace() );
            }
            else
            {
                return new Cookie( CookieAction.None, this, GetStackTrace() );
            }
        }

        /// <summary>
        /// Acquires a writer lock.
        /// </summary>
        /// <returns>An object (considered opaque by client code) that should be disposed
        /// once the lock is not needed.</returns>
        /// <remarks>It is strongly recommended to use the <b>using</b> construct with
        /// this method.</remarks>
        public Cookie AcquireWriteLock()
        {
            if ( this.@lock == null ) return new Cookie( CookieAction.None, null, null );

            CookieAction action;

            if ( !@lock.IsWriteLockHeld )
            {
                if ( !@lock.IsUpgradeableReadLockHeld )
                {
                    if ( @lock.IsReadLockHeld )
                        throw new InvalidOperationException(
                            string.Format( "Cannot acquire a write lock on {{{0}}}, because the current thread " +
                                           "already holds a read lock on that object, and acquiring a write lock may cause a deadlock.",
                                           @lock ) );

                    action = CookieAction.ExitWriteAndUpgradeableReadLock;

                    DeadlockMonitor.EnterWaiting( @lock, writeLock, this.lockedObject );
                    DeadlockMonitor.EnterWaiting( @lock, upgradableReadLock, this.lockedObject );

                    for ( int i = 0; !@lock.TryEnterUpgradeableReadLock( WarningTimeout ); i++ )
                    {
                        if ( i == 0 )
                        {
                            Debug.Print( "Acquiring an upgradeable read lock on {0} from thread {1} ({2}) is taking longer than expected.",
                                         this.lockedObject, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name );
                        }

                        DetectProblems( i );
                    }

                    DeadlockMonitor.ConvertWaitingToAcquired( @lock, upgradableReadLock, this.lockedObject );
                    DeadlockMonitor.ExitWaiting( @lock, writeLock );
                }
                else
                {
                    action = CookieAction.ExitWriteLock;
                }

                DeadlockMonitor.EnterWaiting( @lock, writeLock, this.lockedObject );
                DeadlockMonitor.EnterWaiting( @lock, readLock, this.lockedObject );
                DeadlockMonitor.EnterWaiting( @lock, upgradableReadLock, this.lockedObject );
                for ( int i = 0; !@lock.TryEnterWriteLock( WarningTimeout ); i++ )
                {
                    if ( i == 0 )
                    {
                        Debug.Print( "Acquiring a write lock on {0} from thread {1} ({2}) is taking longer than expected.",
                                     this.lockedObject, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name );
                    }

                    DetectProblems( i );
                }
                DeadlockMonitor.ConvertWaitingToAcquired( @lock, writeLock, this.lockedObject );
                DeadlockMonitor.ExitWaiting( @lock, readLock );
                DeadlockMonitor.ExitWaiting( @lock, upgradableReadLock );
            }
            else
            {
                action = CookieAction.None;
            }

            return new Cookie( action, this, GetStackTrace() );
        }

        /// <summary>
        /// Acquires a reader lock.
        /// </summary>
        /// <returns>An object (considered opaque by client code) that should be disposed
        /// once the lock is not needed.</returns>
        /// <remarks>It is strongly recommended to use the <b>using</b> construct with
        /// this method.</remarks>
        public Cookie AcquireReadLock()
        {
            if ( this.@lock == null ) return new Cookie( CookieAction.None, null, null );

            CookieAction action;

            if ( !@lock.IsReadLockHeld && !@lock.IsUpgradeableReadLockHeld && !@lock.IsWriteLockHeld )
            {
                action = CookieAction.ExitReadLock;
                DeadlockMonitor.EnterWaiting( @lock, writeLock, lockedObject );
                if ( !@lock.TryEnterReadLock( WarningTimeout ) )
                {
                    Debug.Print( "Acquiring a read lock on {0} from thread {1} ({2}) is taking longer than expected.",
                                 lockedObject, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name );
                    DeadlockMonitor.DetectDeadlocks();
                    @lock.EnterReadLock();
                }
                DeadlockMonitor.ExitWaiting( @lock, writeLock );
                DeadlockMonitor.EnterAcquired( @lock, readLock, lockedObject );
            }
            else
            {
                action = CookieAction.None;
            }

            return new Cookie( action, this, GetStackTrace() );
        }

        /// <summary>
        /// Acquires an upgradable reader lock.
        /// </summary>
        /// <returns>An object (considered opaque by client code) that should be disposed
        /// once the lock is not needed.</returns>
        /// <remarks>It is strongly recommended to use the <b>using</b> construct with
        /// this method.</remarks>
        public Cookie AcquireUpgradableReadLock()
        {
            if ( this.@lock == null ) return new Cookie( CookieAction.None, null, null );

            CookieAction action;

            if ( !@lock.IsUpgradeableReadLockHeld && !@lock.IsWriteLockHeld )
            {
                if ( @lock.IsReadLockHeld )
                    throw new InvalidOperationException(
                        string.Format( "Cannot acquire a write lock on {{{0}}}, because the current thread " +
                                       "already holds a read lock on that object, and acquiring a write lock may cause a deadlock.",
                                       @lock ) );

                action = CookieAction.ExitUpgrableReaderLock;

                DeadlockMonitor.EnterWaiting( @lock, upgradableReadLock, lockedObject );
                DeadlockMonitor.EnterWaiting( @lock, writeLock, lockedObject );

                if ( !@lock.TryEnterUpgradeableReadLock( WarningTimeout ) )
                {
                    Debug.Print( "Acquiring a read lock on {0} from thread {1} ({2}) is taking longer than expected.",
                                 lockedObject, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name );

                    DeadlockMonitor.DetectDeadlocks();
                    @lock.EnterUpgradeableReadLock();
                }
                DeadlockMonitor.ConvertWaitingToAcquired( @lock, upgradableReadLock, lockedObject );
                DeadlockMonitor.ExitWaiting( @lock, writeLock );
            }
            else
            {
                action = CookieAction.None;
            }

            return new Cookie( action, this, GetStackTrace() );
        }

        internal enum CookieAction
        {
            None,
            ExitWriteLock,
            ExitWriteAndUpgradeableReadLock,
            ExitReadLock,
            EnterWriteLock,
            ExitUpgrableReaderLock
        }

        /// <summary>
        /// Cookie returned by methods <see cref="ReaderWriterLockWrapper.AcquireReadLock"/>,
        /// <see cref="ReaderWriterLockWrapper.AcquireObserverLock"/>, <see cref="ReaderWriterLockWrapper.AcquireUpgradableReadLock"/>
        /// and <see cref="ReaderWriterLockWrapper.AcquireWriteLock"/>. Disposing the cookie has the effect of releasing the lock.
        /// </summary>
        public class Cookie : IDisposable
        {
            private readonly ReaderWriterLockWrapper parent;
            private CookieAction action;
            private readonly StackTrace stackTrace;

            internal Cookie( CookieAction action, ReaderWriterLockWrapper parent, StackTrace stackTrace )
            {
                this.action = action;
                this.parent = parent;
                this.stackTrace = stackTrace;
            }

            /// <inheritdoc />
            public void Dispose()
            {
                switch ( this.action )
                {
                    case CookieAction.None:
                        break;

                    case CookieAction.ExitReadLock:
                        this.parent.@lock.ExitReadLock();
                        DeadlockMonitor.ExitAcquired( this.parent.@lock, readLock );
                        break;

                    case CookieAction.ExitWriteLock:
                        this.parent.@lock.ExitWriteLock();
                        DeadlockMonitor.ExitAcquired( this.parent.@lock, writeLock );
                        break;

                    case CookieAction.ExitWriteAndUpgradeableReadLock:
                        this.parent.@lock.ExitWriteLock();
                        this.parent.@lock.ExitUpgradeableReadLock();
                        DeadlockMonitor.ExitAcquired( this.parent.@lock, writeLock );
                        DeadlockMonitor.ExitAcquired( this.parent.@lock, upgradableReadLock );
                        break;

                    case CookieAction.EnterWriteLock:
                        DeadlockMonitor.EnterWaiting( this.parent.@lock, readLock, this.parent.lockedObject );
                        DeadlockMonitor.EnterWaiting( this.parent.@lock, writeLock, this.parent.lockedObject );
                        DeadlockMonitor.EnterWaiting( this.parent.@lock, upgradableReadLock, this.parent.lockedObject );

                        if ( !this.parent.@lock.TryEnterWriteLock( WarningTimeout ) )
                        {
                            Debug.Print( "Reacquiring a write lock on {0} from thread {1} ({2}) is taking longer than expected.",
                                         this.parent.lockedObject, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name );
                            DeadlockMonitor.DetectDeadlocks();
                            this.parent.@lock.EnterWriteLock();
                        }
                        DeadlockMonitor.ConvertWaitingToAcquired( this.parent.@lock, writeLock, this.parent.lockedObject );
                        DeadlockMonitor.ExitWaiting( this.parent.@lock, readLock );
                        DeadlockMonitor.ExitWaiting( this.parent.@lock, upgradableReadLock );

                        break;

                    case CookieAction.ExitUpgrableReaderLock:
                        this.parent.@lock.ExitUpgradeableReadLock();
                        DeadlockMonitor.ExitAcquired( this.parent.@lock, upgradableReadLock );
                        break;
                }

                this.action = CookieAction.None;

                GC.SuppressFinalize( this );
            }

            /// <summary>
            /// Finalizer. Checks that the cookie has been disposed properly.
            /// </summary>
            ~Cookie()
            {
                if ( this.action != CookieAction.None )
                {
                    throw new LockNotDisposedException(
                        string.Format( "Lock cookie on object '{0}' was not disposed properly. Pending action: {1}. Stack trace: {2}.",
                                       this.parent.lockedObject,
                                       this.action,
                                       this.stackTrace != null
                                           ? Environment.NewLine + this.stackTrace.ToString()
                                           :
                                               "unknown" ) );
                }
            }
        }
    }
}