using System;
using System.Collections;
using System.Collections.Generic;
using Threading;

namespace ContactManager.Framework
{
    /// <summary>
    /// Synchronized dictionary keeping only weak references to the items (but not to their keys)
    /// it contains.
    /// </summary>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <typeparam name="TValue">Type of values.</typeparam>
    public class WeakDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReaderWriterSynchronized
        where TValue : class, IObservableFinalize
    {
        private readonly ReaderWriterLockWrapper @lock;
        private readonly Dictionary<TKey, WeakRef> impl = new Dictionary<TKey, WeakRef>();

        /// <summary>
        /// Initializes a new <see cref="WeakDictionary{TKey,TValue}"/> with a new lock.
        /// </summary>
        public WeakDictionary()
        {
            this.@lock = new ReaderWriterLockWrapper( this );
        }

        /// <summary>
        /// Initializes a new <see cref="WeakDictionary{TKey,TValue}"/> with an existing lock.
        /// </summary>
        /// <param name="lock">An existing lock.</param>
        public WeakDictionary( ReaderWriterLockWrapper @lock )
        {
            if ( @lock == null ) throw new ArgumentNullException( "lock" );
            this.@lock = @lock;
        }

        /// <inheritdoc />
        [ReadLock]
        public bool ContainsKey( TKey key )
        {
            WeakRef weakRef;

            if ( this.impl.TryGetValue( key, out weakRef ) )
            {
                return weakRef.IsAlive;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc />
        [WriteLock]
        public void Add( TKey key, TValue value )
        {
            this.impl.Add( key, new WeakRef( this, key, value ) );
        }

        /// <inheritdoc />
        [WriteLock]
        public bool Remove( TKey key )
        {
            return this.impl.Remove( key );
        }

        /// <inheritdoc />
        [ReadLock]
        public bool TryGetValue( TKey key, out TValue value )
        {
            WeakRef weakRef;
            if ( this.impl.TryGetValue( key, out weakRef ) )
            {
                if ( weakRef.IsAlive )
                {
                    value = weakRef.Target;
                    return true;
                }
            }

            value = null;
            return false;
        }

        /// <inheritdoc />
        public TValue this[ TKey key ]
        {
            get
            {
                TValue value;
                if ( !this.TryGetValue( key, out value ) )
                    throw new KeyNotFoundException();

                return value;
            }

            [WriteLock]
            set { this.impl[key] = new WeakRef( this, key, value ); }
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys
        {
            get { throw new NotImplementedException(); }
        }

        /// <inheritdoc />
        public ICollection<TValue> Values
        {
            get { throw new NotImplementedException(); }
        }

        /// <inheritdoc />
        public void Add( KeyValuePair<TKey, TValue> item )
        {
            this.Add( item.Key, item.Value );
        }

        /// <inheritdoc />
        [WriteLock]
        public void Clear()
        {
            this.impl.Clear();
        }

        /// <inheritdoc />
        public bool Contains( KeyValuePair<TKey, TValue> item )
        {
            return this.ContainsKey( item.Key );
        }

        /// <inheritdoc />
        public void CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool Remove( KeyValuePair<TKey, TValue> item )
        {
            return this.Remove( item.Key );
        }

        /// <inheritdoc />
        [ReadLock]
        public int Count
        {
            get { return this.impl.Count; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        public ReaderWriterLockWrapper Lock
        {
            get { return this.@lock; }
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach ( KeyValuePair<TKey, WeakRef> pair in impl )
            {
                if ( pair.Value.IsAlive )
                    yield return new KeyValuePair<TKey, TValue>( pair.Key, pair.Value.Target );
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// A node of a <see cref="WeakDictionary{TKey,TValue}"/> containing
        /// a reference to the parent dictionary and the key of the current node.
        /// </summary>
        private class WeakRef
        {
            private readonly WeakDictionary<TKey, TValue> parent;
            private readonly TKey key;
            private readonly WeakReference weakRef;

            public WeakRef( WeakDictionary<TKey, TValue> parent, TKey key, TValue value )
            {
                this.key = key;
                this.weakRef = new WeakReference( value );
                this.parent = parent;
                value.Finalized += OnValueCollected;
            }

            private void OnValueCollected( object sender, EventArgs e )
            {
                this.parent.Remove( this.key );
            }

            public bool IsAlive
            {
                get { return this.weakRef.IsAlive; }
            }

            public TValue Target
            {
                get { return (TValue) this.weakRef.Target; }
            }
        }
    }
}