using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Threading
{
    /// <summary>
    /// A dictionary that is both observable (by deriving the class and implementing handlers)
    /// and reader-writer synchronized.
    /// </summary>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <typeparam name="TValue">Type of values.</typeparam>
    /// <remarks>
    /// To make this class compatible with WPF observable collections, we need to assign indices
    /// to items. Indices are assigned when items are added, but they can change when
    /// an item is removed.
    /// </remarks>
    internal class ObservableSynchronizedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReaderWriterSynchronized, IList<KeyValuePair<TKey, TValue>>
    {
        private int revision;
        private readonly Dictionary<TKey, Slot> dictionary;
        private readonly Dictionary<int, TKey> slotToKey;
        private readonly ReaderWriterLockWrapper @lock;

        /// <summary>
        /// Initializes a new <see cref="ObservableSynchronizedDictionary{TKey,TValue}"/> and specifies an existing lock
        /// and the default equality comparer.
        /// </summary>
        /// <param name="lock">An existing lock.</param>
        public ObservableSynchronizedDictionary( ReaderWriterLockWrapper @lock )
        {
            if ( @lock == null ) throw new ArgumentNullException( "lock" );

            this.@lock = @lock;
            this.dictionary = new Dictionary<TKey, Slot>();
            this.slotToKey = new Dictionary<int, TKey>();
        }

        /// <summary>
        /// Initializes a new <see cref="ObservableSynchronizedDictionary{TKey,TValue}"/> and specifies an existing
        /// lock and a comparer.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="lock">An existing lock.</param>
        public ObservableSynchronizedDictionary( IEqualityComparer<TKey> comparer, ReaderWriterLockWrapper @lock )
        {
            if ( comparer == null ) throw new ArgumentNullException( "comparer" );
            if ( @lock == null ) throw new ArgumentNullException( "lock" );

            this.@lock = @lock;
            this.dictionary = new Dictionary<TKey, Slot>( comparer );
            this.slotToKey = new Dictionary<int, TKey>();
        }


        /// <summary>
        /// Initializes a new <see cref="ObservableSynchronizedDictionary{TKey,TValue}"/> with a new lock and
        /// the default equality comparer.
        /// </summary>
        public ObservableSynchronizedDictionary()
        {
            this.@lock = new ReaderWriterLockWrapper( this );
        }

        /// <summary>
        /// Initializes a new <see cref="ObservableSynchronizedDictionary{TKey,TValue}"/> with a new lock, but specifies
        /// the equality comparer.
        /// </summary>
        /// <param name="comparer">An equality comparer.</param>
        public ObservableSynchronizedDictionary( IEqualityComparer<TKey> comparer )
        {
            if ( comparer == null ) throw new ArgumentNullException( "comparer" );
            this.dictionary = new Dictionary<TKey, Slot>( comparer );
            this.slotToKey = new Dictionary<int, TKey>();
            this.@lock = new ReaderWriterLockWrapper( this );
        }

        /// <inheritdoc />
        public ReaderWriterLockWrapper Lock
        {
            get { return this.@lock; }
        }

        /// <inheritdoc />
        [ReadLock]
        public bool ContainsKey( TKey key )
        {
            return this.dictionary.ContainsKey( key );
        }

        /// <inheritdoc />
        [WriteLock]
        public int Add( TKey key, TValue value )
        {
            int index = this.dictionary.Count;
            this.dictionary.Add( key, new Slot( key, value, index ) );
            this.slotToKey.Add( index, key );
            this.revision++;

            this._OnItemAdded( key, value, index );
            return index;
        }

        void IDictionary<TKey, TValue>.Add( TKey key, TValue value )
        {
            this.Add( key, value );
        }

        /// <summary>
        /// Adds an item to the dictionary if it does not already contain 
        /// any item with the same key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="item">Value.</param>
        /// <returns><b>true</b> if the item was added, otherwise <b>false</b>.</returns>
        public bool AddIfAbsent( TKey key, TValue item )
        {
            using ( this.AcquireUpgradableReadLock() )
            {
                if ( this.dictionary.ContainsKey( key ) )
                    return false;
                else
                {
                    this.Add( key, item );
                    return true;
                }
            }
        }


        [ObserverLock]
        private void _OnItemAdded( TKey key, TValue value, int index )
        {
            this.OnItemAdded( key, value, index );
        }

        /// <summary>
        /// Method invoked when an item has been added to the dictionary.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <param name="index">Index.</param>
        protected virtual void OnItemAdded( TKey key, TValue value, int index )
        {
        }

        /// <inheritdoc />
        [WriteLock]
        public bool Remove( TKey key )
        {
            Slot slot;

            if ( !this.dictionary.TryGetValue( key, out slot ) )
            {
                return false;
            }
            else
            {
                this.dictionary.Remove( key );
                this.slotToKey.Remove( slot.Index );

                // This is too stupid. We have to change the indexes of all nodes having a superior index.
                foreach ( Slot otherSlot in this.dictionary.Values )
                {
                    if ( otherSlot.Index > slot.Index )
                    {
                        this.slotToKey.Remove( otherSlot.Index );
                        otherSlot.Index--;
                        this.slotToKey.Add( otherSlot.Index, otherSlot.Key );
                    }
                }

                this.revision++;

                this._OnItemRemoved( key, slot.Value, slot.Index );

                return true;
            }
        }

        [ObserverLock]
        private void _OnItemRemoved( TKey key, TValue value, int index )
        {
            this.OnItemRemoved( key, value, index );
        }

        /// <summary>
        /// Method invoked when an item has been removed from the dictionary.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <param name="index">Index.</param>
        protected virtual void OnItemRemoved( TKey key, TValue value, int index )
        {
        }

        /// <inheritdoc />
        [ReadLock]
        public bool TryGetValue( TKey key, out TValue value )
        {
            Slot slot;
            bool success = this.dictionary.TryGetValue( key, out slot );

            if ( success )
            {
                value = slot.Value;
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
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
            set
            {
                Slot slot;
                if ( this.dictionary.TryGetValue( key, out slot ) )
                {
                    TValue oldValue = slot.Value;

                    slot.Value = value;

                    this.revision++;

                    this._OnItemReplaced( key, oldValue, value, slot.Index );
                }
                else
                {
                    int index = this.dictionary.Count;
                    this.dictionary.Add( key, new Slot( key, value, index ) );
                    this.slotToKey.Add( index, key );

                    this.revision++;

                    this._OnItemAdded( key, value, index );
                }
            }
        }


        /// <inheritdoc />
        public KeyValuePair<TKey, TValue> this[ int index ]
        {
            [ReadLock]
            get
            {
                TKey key = this.slotToKey[index];
                return new KeyValuePair<TKey, TValue>( key, this.dictionary[key].Value );
            }

            [WriteLock]
            set
            {
                TKey key = this.slotToKey[index];
                Slot slot = this.dictionary[key];
                this.dictionary.Remove( key );
                slot.Key = value.Key;
                slot.Value = value.Value;
                this.dictionary.Add( key, slot );
                this.slotToKey[index] = value.Key;
            }
        }

        [ObserverLock]
        private void _OnItemReplaced( TKey key, TValue oldValue, TValue newValue, int index )
        {
            this.OnItemReplaced( key, oldValue, newValue, index );
        }

        /// <summary>
        /// Method invoked when an item has been replaced by another.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        /// <param name="index">Index.</param>
        protected virtual void OnItemReplaced( TKey key, TValue oldValue, TValue newValue, int index )
        {
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys
        {
            get { return new KeyCollection( this ); }
        }

        /// <inheritdoc />
        public ICollection<TValue> Values
        {
            get { return new ValueCollection( this ); }
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
            this.dictionary.Clear();
            this.slotToKey.Clear();
            this.revision++;

            this._OnItemsCleared();
        }

        [ObserverLock]
        private void _OnItemsCleared()
        {
            this.OnItemsCleared();
        }

        /// <summary>
        /// Method invoked when all items have been cleared from the dictionary.
        /// </summary>
        protected virtual void OnItemsCleared()
        {
        }

        /// <inheritdoc />
        public bool Contains( KeyValuePair<TKey, TValue> item )
        {
            return this.ContainsKey( item.Key );
        }

        /// <inheritdoc />
        [ReadLock]
        public void CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
        {
            int index = 0;
            foreach ( KeyValuePair<TKey, Slot> pair in dictionary )
            {
                array[index] = new KeyValuePair<TKey, TValue>( pair.Key, pair.Value.Value );
                index++;
            }
        }

        /// <inheritdoc />
        public bool Remove( KeyValuePair<TKey, TValue> item )
        {
            return this.Remove( item.Key );
        }

        /// <inheritdoc />
        public int Count
        {
            [ReadLock]
            get { return this.dictionary.Count; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return false; }
        }

        private IEnumerator<KeyValuePair<TKey, TValue>> GetEnumeratorImpl()
        {
            foreach ( KeyValuePair<TKey, Slot> pair in this.dictionary )
            {
                yield return new KeyValuePair<TKey, TValue>( pair.Key, pair.Value.Value );
            }
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if ( this.@lock.IsReadLockHeld )
            {
                // If the caller has a readlock, we return an ungarded enumerator.
                // We suppose that the caller will not release the lock during enumeration.
                return this.GetEnumeratorImpl();
            }
            else
            {
                // Otherwise, we return an enumerator to a copy of the collection.

                using ( this.AcquireReadLock() )
                {
                    KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[this.dictionary.Count];
                    this.CopyTo( array, 0 );
                    return new ArrayEnumerator<KeyValuePair<TKey, TValue>>( array );
                }
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private class Slot
        {
            public Slot( TKey key, TValue value, int index )
            {
                this.Index = index;
                this.Value = value;
                this.Key = key;
            }

            public int Index;
            public TValue Value;
            public TKey Key;
        }

        private class ArrayEnumerator<T> : IEnumerator<T>
        {
            private readonly T[] array;
            private int nextPosition;

            public ArrayEnumerator( T[] array )
            {
                this.array = array;
            }

            public void Dispose()
            {
            }


            public bool MoveNext()
            {
                if ( this.nextPosition < this.array.Length )
                {
                    this.nextPosition++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                this.nextPosition = 0;
            }

            public T Current
            {
                get { return this.array[this.nextPosition - 1]; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        /// <summary>
        /// Collection of keys of an <see cref="ObservableSynchronizedDictionary{TKey,TValue}"/>.
        /// </summary>
        public class KeyCollection : ICollection<TKey>, IReaderWriterSynchronized
        {
            private readonly ObservableSynchronizedDictionary<TKey, TValue> parent;

            internal KeyCollection( ObservableSynchronizedDictionary<TKey, TValue> parent )
            {
                this.parent = parent;
            }

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <param name="item"></param>
            public void Add( TKey item )
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Not supported.
            /// </summary>
            public void Clear()
            {
                throw new NotSupportedException();
            }

            /// <inheritdoc />
            public bool Contains( TKey item )
            {
                return this.parent.ContainsKey( item );
            }

            /// <inheritdoc />
            [ReadLock]
            public void CopyTo( TKey[] array, int arrayIndex )
            {
                this.parent.dictionary.Keys.CopyTo( array, arrayIndex );
            }

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public bool Remove( TKey item )
            {
                throw new NotSupportedException();
            }

            /// <inheritdoc />
            public int Count
            {
                get { return this.parent.Count; }
            }

            /// <inheritdoc />
            public bool IsReadOnly
            {
                get { return true; }
            }

            /// <inheritdoc />
            public ReaderWriterLockWrapper Lock
            {
                get { return this.parent.@lock; }
            }


            /// <inheritdoc />
            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
            {
                if ( this.parent.@lock.IsReadLockHeld )
                {
                    return this.parent.dictionary.Keys.GetEnumerator();
                }
                else
                {
                    using ( this.parent.AcquireReadLock() )
                    {
                        TKey[] array = new TKey[this.parent.Count];
                        this.parent.dictionary.Keys.CopyTo( array, 0 );
                        return new ArrayEnumerator<TKey>( array );
                    }
                }
            }

            /// <inheritdoc />
            public IEnumerator GetEnumerator()
            {
                return ((IEnumerable<TKey>) this).GetEnumerator();
            }
        }

        /// <summary>
        /// Collection of values of an <see cref="SynchronizedKeyedCollection{TKey,TValue}"/>.
        /// </summary>
        public class ValueCollection : ICollection<TValue>, IReaderWriterSynchronized
        {
            private readonly ObservableSynchronizedDictionary<TKey, TValue> parent;

            internal ValueCollection( ObservableSynchronizedDictionary<TKey, TValue> parent )
            {
                this.parent = parent;
            }

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <param name="item"></param>
            public void Add( TValue item )
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Not supported.
            /// </summary>
            public void Clear()
            {
                throw new NotSupportedException();
            }

            /// <inheritdoc />
            public bool Contains( TValue item )
            {
                return Enumerable.Contains( this, item );
            }

            /// <inheritdoc />
            [ReadLock]
            public void CopyTo( TValue[] array, int arrayIndex )
            {
                if ( array == null ) throw new ArgumentNullException( "array" );

                Slot[] slots = new Slot[array.Length];
                this.parent.dictionary.Values.CopyTo( slots, arrayIndex );
                for ( int i = 0; i < slots.Length; i++ )
                {
                    array[i] = slots[i].Value;
                }
            }

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public bool Remove( TValue item )
            {
                throw new NotSupportedException();
            }

            /// <inheritdoc />
            public int Count
            {
                get
                {
                    int count = this.parent.Count;
                    return count;
                }
            }

            /// <inheritdoc />
            public bool IsReadOnly
            {
                get { return true; }
            }

            /// <inheritdoc />
            public ReaderWriterLockWrapper Lock
            {
                get { return this.parent.@lock; }
            }

            private IEnumerator<TValue> GetEnumeratorImpl()
            {
                foreach ( Slot slot in this.parent.dictionary.Values )
                {
                    yield return slot.Value;
                }
            }

            /// <inheritdoc />
            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            {
                if ( this.parent.@lock.IsReadLockHeld )
                {
                    return this.GetEnumeratorImpl();
                }
                else
                {
                    using ( this.parent.AcquireReadLock() )
                    {
                        TValue[] array = new TValue[this.parent.Count];
                        this.CopyTo( array, 0 );
                        return new ArrayEnumerator<TValue>( array );
                    }
                }
            }

            /// <inheritdoc />
            public IEnumerator GetEnumerator()
            {
                return ((IEnumerable<TValue>) this).GetEnumerator();
            }
        }

        #region Implementation of IList<KeyValuePair<TKey,TValue>>

        [ReadLock]
        public int IndexOf( TKey key )
        {
            Slot slot;
            if ( this.dictionary.TryGetValue( key, out slot ) )
            {
                return slot.Index;
            }
            else
            {
                return -1;
            }
        }


        public int IndexOf( KeyValuePair<TKey, TValue> item )
        {
            Slot slot;
            if ( this.dictionary.TryGetValue( item.Key, out slot ) )
            {
                if ( (item.Value == null && slot.Value == null) ||
                     item.Value.Equals( slot.Value ) )
                    return slot.Index;
                else
                    return -1;
            }
            else
            {
                return -1;
            }
        }

        void IList<KeyValuePair<TKey, TValue>>.Insert( int index, KeyValuePair<TKey, TValue> item )
        {
            throw new NotSupportedException();
        }

        [WriteLock]
        public void RemoveAt( int index )
        {
            TKey key = this.slotToKey[index];
            this.Remove( key );
        }

        #endregion
    }
}