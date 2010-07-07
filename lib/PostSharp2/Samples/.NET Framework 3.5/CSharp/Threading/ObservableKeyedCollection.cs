using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Threading
{
    /// <summary>
    /// A keyed collection that is both observable through <see cref="INotifyCollectionChanged"/>,
    /// and reader-writer synchronized.
    /// </summary>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    /// <typeparam name="TValue">Type of values.</typeparam>
    [Serializable]
    public abstract class SynchronizedKeyedCollection<TKey, TValue> : ICollection<TValue>,
                                                                      IReaderWriterSynchronized,
                                                                      INotifyCollectionChanged,
                                                                      IList
    {
        private readonly SynchronizedDictionary dictionary;

        /// <summary>
        /// Initializes a new <see cref="SynchronizedKeyedCollection{TKey,TValue}"/> and specifies an existing lock
        /// and the default equality comparer.
        /// </summary>
        /// <param name="lock">An existing lock.</param>
        protected SynchronizedKeyedCollection( ReaderWriterLockWrapper @lock )
        {
            if ( @lock == null ) throw new ArgumentNullException( "lock" );

            this.dictionary = new SynchronizedDictionary( this, @lock );
        }

        /// <summary>
        /// Initializes a new <see cref="SynchronizedKeyedCollection{TKey,TValue}"/> and specifies an existing
        /// lock and a comparer.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="lock">An existing lock.</param>
        protected SynchronizedKeyedCollection( IEqualityComparer<TKey> comparer, ReaderWriterLockWrapper @lock )
        {
            if ( comparer == null ) throw new ArgumentNullException( "comparer" );
            if ( @lock == null ) throw new ArgumentNullException( "lock" );

            this.dictionary = new SynchronizedDictionary( this, comparer, @lock );
        }

        /// <summary>
        /// Initializes a new <see cref="SynchronizedKeyedCollection{TKey,TValue}"/> with a new lock and
        /// the default equality comparer.
        /// </summary>
        protected SynchronizedKeyedCollection()
        {
            this.dictionary = new SynchronizedDictionary( this, new ReaderWriterLockWrapper( this ) );
        }

        /// <summary>
        /// Initializes a new <see cref="SynchronizedKeyedCollection{TKey,TValue}"/> with a new lock, but specifies
        /// the equality comparer.
        /// </summary>
        /// <param name="comparer">An equality comparer.</param>
        protected SynchronizedKeyedCollection( IEqualityComparer<TKey> comparer )
        {
            if ( comparer == null ) throw new ArgumentNullException( "comparer" );
            this.dictionary = new SynchronizedDictionary( this, comparer, new ReaderWriterLockWrapper( this ) );
        }

        /// <inheritdoc />
        public ReaderWriterLockWrapper Lock
        {
            get { return this.dictionary.Lock; }
        }

        /// <inheritdoc />
        [DispatchEvent]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Gets the key of a given item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The key of a given item.</returns>
        protected abstract TKey GetKeyForItem( TValue item );


        /// <inheritdoc />
        public virtual int Add( TValue item )
        {
            return this.dictionary.Add( this.GetKeyForItem( item ), item );
        }

        void ICollection<TValue>.Add( TValue item )
        {
            this.Add( item );
        }

        /// <summary>
        /// Adds an item to the dictionary if it does not already contain 
        /// any item with the same key.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <returns><b>true</b> if the item was added, otherwise <b>false</b>.</returns>
        public bool AddIfAbsent( TValue item )
        {
            if ( this.Contains( item ) )
            {
                return false;
            }
            else
            {
                this.Add( item );
                return true;
            }
        }

        /// <inheritdoc />
        int IList.Add( object value )
        {
            return this.Add( (TValue) value );
        }

        /// <inheritdoc />
        bool IList.Contains( object value )
        {
            return this.Contains( (TValue) value );
        }

        /// <inheritdoc />
        public virtual void Clear()
        {
            this.dictionary.Clear();
        }

        /// <inheritdoc />
        int IList.IndexOf( object value )
        {
            return this.dictionary.IndexOf( GetKeyForItem( (TValue) value ) );
        }

        /// <inheritdoc />
        void IList.Insert( int index, object value )
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        void IList.Remove( object value )
        {
            this.Remove( (TValue) value );
        }

        /// <inheritdoc />
        void IList.RemoveAt( int index )
        {
            this.dictionary.RemoveAt( index );
        }

        /// <inheritdoc />
        object IList.this[ int index ]
        {
            get { return this[index]; }
            set { this[index] = (TValue) value; }
        }

        /// <inheritdoc />
        public bool Contains( TKey key )
        {
            return this.dictionary.ContainsKey( key );
        }

        /// <inheritdoc />
        public bool Contains( TValue item )
        {
            return this.dictionary.Contains( new KeyValuePair<TKey, TValue>( this.GetKeyForItem( item ), item ) );
        }

        /// <inheritdoc />
        public bool ContainsKey( TKey key )
        {
            return this.dictionary.ContainsKey( key );
        }

        /// <inheritdoc />
        public TValue this[ TKey key ]
        {
            get
            {
                TValue value;
                this.dictionary.TryGetValue( key, out value );
                return value;
            }
        }

        /// <inheritdoc />
        public TValue this[ int index ]
        {
            get { return this.dictionary[index].Value; }
            set { this.dictionary[index] = new KeyValuePair<TKey, TValue>( GetKeyForItem( value ), value ); }
        }

        /// <inheritdoc />
        public void CopyTo( TValue[] array, int arrayIndex )
        {
            this.dictionary.Values.CopyTo( array, arrayIndex );
        }

        /// <inheritdoc />
        public virtual bool Remove( TValue item )
        {
            return this.dictionary.Remove( new KeyValuePair<TKey, TValue>( this.GetKeyForItem( item ), item ) );
        }

        /// <inheritdoc />
        public virtual bool Remove( TKey key )
        {
            return this.dictionary.Remove( key );
        }

        [ReadLock]
        void ICollection.CopyTo( Array array, int index )
        {
            int i = index;
            foreach ( TValue value in this )
            {
                array.SetValue( value, i );
                i++;
            }
        }

        /// <inheritdoc />
        public int Count
        {
            get { return this.dictionary.Count; }
        }

        /// <inheritdoc />
        object ICollection.SyncRoot
        {
            get { return this; }
        }

        /// <inheritdoc />
        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        bool IList.IsFixedSize
        {
            get { return false; }
        }

        /// <inheritdoc />
        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return this.dictionary.Values.GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<TValue>) this).GetEnumerator();
        }

        [Serializable]
        private sealed class SynchronizedDictionary : ObservableSynchronizedDictionary<TKey, TValue>
        {
            private readonly SynchronizedKeyedCollection<TKey, TValue> parent;

            public SynchronizedDictionary( SynchronizedKeyedCollection<TKey, TValue> parent, ReaderWriterLockWrapper @lock )
                : base( @lock )
            {
                this.parent = parent;
            }

            public SynchronizedDictionary( SynchronizedKeyedCollection<TKey, TValue> parent,
                                           IEqualityComparer<TKey> comparer, ReaderWriterLockWrapper @lock )
                : base( comparer, @lock )
            {
                this.parent = parent;
            }

            protected override void OnItemAdded( TKey key, TValue value, int index )
            {
                if ( this.parent.CollectionChanged != null )
                {
                    this.parent.CollectionChanged( this.parent,
                                                   new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, value, index ) );
                }
            }

            protected override void OnItemRemoved( TKey key, TValue value, int index )
            {
                if ( this.parent.CollectionChanged != null )
                {
                    this.parent.CollectionChanged( this.parent, new NotifyCollectionChangedEventArgs(
                                                                    NotifyCollectionChangedAction.Remove, value, index ) );
                }
            }

            protected override void OnItemReplaced( TKey key, TValue oldValue, TValue newValue, int index )
            {
                if ( this.parent.CollectionChanged != null )
                {
                    this.parent.CollectionChanged( this.parent, new NotifyCollectionChangedEventArgs(
                                                                    NotifyCollectionChangedAction.Replace, newValue, oldValue, index ) );
                }
            }

            protected override void OnItemsCleared()
            {
                if ( this.parent.CollectionChanged != null )
                {
                    this.parent.CollectionChanged( this.parent,
                                                   new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
                }
            }
        }
    }
}