using System;
using System.Threading;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Serialization;

namespace Threading
{
    /// <summary>
    /// Custom attribute that, when applied on a method, makes it asynchronous, i.e.
    /// queued to the <see cref="ThreadPool"/>.
    /// </summary>
    [Serializable]
    [MethodInterceptionAspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
    public sealed class AsyncAttribute : MethodInterceptionAspect
    {
        /// <summary>
        /// Initializes a new <see cref="AsyncAttribute"/>.
        /// </summary>
        public AsyncAttribute()
        {
            this.AspectPriority = 1;
        }

        /// <inheritdoc />
        public override void OnInvoke( MethodInterceptionArgs args )
        {
            ThreadPool.QueueUserWorkItem(
                delegate { args.Proceed(); } );
        }
    }
}