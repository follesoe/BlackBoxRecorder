using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Dependencies;
using PostSharp.Aspects.Serialization;
using PostSharp.Extensibility;

namespace Threading
{
    /// <summary>
    /// Custom attribute that, when applied on a method, specifies that it should be executed in
    /// a reader lock.
    /// </summary>
    /// <remarks>
    /// <para>The current custom attribute can be applied to instance methods of classes implementing
    /// the <see cref="IReaderWriterSynchronized"/> interface.</para>
    /// </remarks>
    [Serializable]
    [MulticastAttributeUsage( MulticastTargets.Method, TargetMemberAttributes = MulticastAttributes.Instance )]
    [OnMethodBoundaryAspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
    [ProvideAspectRole( StandardRoles.Threading )]
    public sealed class WriteLockAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// Initializes a new <see cref="WriteLockAttribute"/> specifying
        /// that write locks will be able to be downgraded to observer locks.
        /// </summary>
        public WriteLockAttribute()
        {
            this.AspectPriority = 2;
        }

        /// <summary>
        /// Handler executed before execution of the method to which the current custom attribute is applied.
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnEntry( MethodExecutionArgs eventArgs )
        {
            eventArgs.MethodExecutionTag = ((IReaderWriterSynchronized) eventArgs.Instance).AcquireWriteLock();
        }

        /// <summary>
        /// Handler executed after execution of the method to which the current custom attribute is applied.
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void OnExit( MethodExecutionArgs eventArgs )
        {
            ((ReaderWriterLockWrapper.Cookie) eventArgs.MethodExecutionTag).Dispose();
        }
    }
}