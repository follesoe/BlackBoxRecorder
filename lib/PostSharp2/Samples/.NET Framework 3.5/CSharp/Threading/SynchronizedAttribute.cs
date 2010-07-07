using System;
using System.Threading;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Serialization;

namespace Threading
{
    /// <summary>
    /// Custom attribute that, when applied on a method, synchronizes its execution
    /// using a simple <see cref="Monitor"/>.
    /// </summary>
    /// <remarks>
    /// Instance methods are synchronized at instance level; static methods are
    /// synchronized at type level.
    /// </remarks>
    [Serializable]
    [OnMethodBoundaryAspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
    public class SynchronizedAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// Initializes a new <see cref="SynchronizedAttribute"/>.
        /// </summary>
        public SynchronizedAttribute()
        {
            this.AspectPriority = 2;
        }

        /// <summary>
        /// Handler executed before execution of the method to which the current custom attribute is applied.
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnEntry( MethodExecutionArgs eventArgs )
        {
            object o = eventArgs.Instance ?? eventArgs.Method.DeclaringType;
            DeadlockMonitor.EnterWaiting( o, null, null );

            if ( !Monitor.TryEnter( o, 200 ) )
            {
                DeadlockMonitor.DetectDeadlocks();
                Monitor.Enter( o );
            }
            DeadlockMonitor.ConvertWaitingToAcquired( o, null, null );
        }

        /// <summary>
        /// Handler executed after execution of the method to which the current custom attribute is applied.
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnExit( MethodExecutionArgs eventArgs )
        {
            object o = eventArgs.Instance ?? eventArgs.Method.DeclaringType;
            Monitor.Exit( o );
            DeadlockMonitor.ExitAcquired( o, null );
        }
    }
}