#region Released to Public Domain by Gael Fraiteur
/*----------------------------------------------------------------------------*
 *   This file is part of samples of PostSharp.                                *
 *                                                                             *
 *   This sample is free software: you have an unlimited right to              *
 *   redistribute it and/or modify it.                                         *
 *                                                                             *
 *   This sample is distributed in the hope that it will be useful,            *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of            *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                      *
 *                                                                             *
 *----------------------------------------------------------------------------*/
#endregion

using System;
using System.ComponentModel;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;

namespace Librarian.WinForms
{
    public class Accessor<T> : IComponent
        where T : class
    {
        private static readonly bool isDisposable = typeof(IDisposable).IsAssignableFrom( typeof(T) );


        /// <summary>
        /// The remote object, or <b>null</b> if the instance has been disposed.
        /// </summary>
        private T remoteObject;

        private readonly ILease lease;

        private readonly Sponsor sponsor = new Sponsor();

        private readonly int hashCode;

        private ISite site;


        /// <summary>
        /// Initializes a new <see cref="Accessor{T}"/>.
        /// </summary>
        /// <param name="remoteObject">An existing remote object (marshalle by reference).</param>
        public Accessor( T remoteObject )
        {
            MarshalByRefObject marshalByRefObject = remoteObject as MarshalByRefObject;

            #region Preconditions

            if ( remoteObject == null )
                throw new ArgumentNullException( "remoteObject" );
            if ( marshalByRefObject == null )
                throw new ArgumentException( "The object should be a MarshalByRefObject.", "remoteObject" );

            #endregion

            this.lease = (ILease) RemotingServices.GetLifetimeService( marshalByRefObject );
            this.lease.Register( this.sponsor );
            this.remoteObject = remoteObject;
            this.hashCode = remoteObject.GetHashCode();
        }

        /// <summary>
        /// Gets the remote object.
        /// </summary>
        public T Value
        {
            get
            {
                this.AssertNotDisposed();
                return this.remoteObject;
            }
        }

        /// <summary>
        /// Determines whether the current instance has been disposed.
        /// </summary>
        public bool IsDisposed { get { return this.remoteObject == null; } }

        /// <summary>
        /// Throws an exception if the current instance has already been disposed.
        /// </summary>
        protected void AssertNotDisposed()
        {
            if ( remoteObject == null )
                throw new ObjectDisposedException( this.GetType().FullName );
        }

        /// <summary>
        /// Disposes the current instance.
        /// </summary>
        /// <param name="disposing"><b>false</b> if the method is called by the
        /// destructor, otherwise <b>true</b>.</param>
        protected virtual void Dispose( bool disposing )
        {
            // Dispose the sponsor.
            try
            {
                this.lease.Unregister( this.sponsor );
                this.sponsor.Dispose();
            }
            catch
            {
            }

            // Dispose the remote object.
            if ( isDisposable )
            {
                ( (IDisposable) this.remoteObject ).Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if ( this.remoteObject != null )
            {
                if ( this.Disposed != null )
                    this.Disposed( this, EventArgs.Empty );

                this.Dispose( true );
                this.remoteObject = null;
            }
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            this.AssertNotDisposed();
            return this.hashCode;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            this.AssertNotDisposed();
            return this.Value.ToString();
        }

        #region IComponent Members

        public event EventHandler Disposed;

        ISite IComponent.Site { get { return this.site; } set { this.site = value; } }

        #endregion

        private class Sponsor : MarshalByRefObject, ISponsor, IDisposable
        {
            private bool disposed;

            public TimeSpan Renewal( ILease lease )
            {
                if ( !this.disposed )
                {
                    return TimeSpan.FromSeconds( 30 );
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            public void Dispose()
            {
                this.disposed = true;
            }
        }
    }
}