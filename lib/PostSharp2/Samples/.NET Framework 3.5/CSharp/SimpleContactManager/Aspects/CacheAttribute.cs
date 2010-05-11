using System;
using System.Collections.Generic;
using System.Threading;
using PostSharp.Aspects;

namespace ContactManager.Aspects
{
    [Serializable]
    public sealed class CacheAttribute : MethodInterceptionAspect
    {
        private static readonly Cache cache = new Cache();
        private MethodFormatStrings formatStrings;

        public override void CompileTimeInitialize(System.Reflection.MethodBase method, AspectInfo aspectInfo)
        {
            this.formatStrings = Formatter.GetMethodFormatStrings(method);
            base.CompileTimeInitialize(method, aspectInfo);
        }

        public override void OnInvoke( MethodInterceptionArgs eventArgs )
        {
            // Compute the cache key.
            string key = this.formatStrings.Format( eventArgs.Instance,
                         eventArgs.Method,
                         eventArgs.Arguments.ToArray() );

            object value;

            if ( !cache.TryGetValue( key, out value ) )
            {
                lock ( this )
                {
                    if ( !cache.TryGetValue( key, out value ) )
                    {
                        eventArgs.Proceed();
                        value = eventArgs.ReturnValue;
                        cache.Add( key, value );
                        return;
                    }
                }
            }

            eventArgs.ReturnValue = value;
        }

        private class Cache
        {
            private readonly ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();
            private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

            public bool TryGetValue( string key, out object value )
            {
                @lock.EnterReadLock();
                bool found = this.dictionary.TryGetValue( key, out value );
                @lock.ExitReadLock();
                return found;
            }

            public void Add( string key, object value )
            {
                @lock.EnterWriteLock();
                this.dictionary.Add( key, value );
                @lock.ExitWriteLock();
            }
        }
    }
}