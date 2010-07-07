using System;
using System.Threading;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Dependencies;
using PostSharp.Aspects.Serialization;

namespace Threading
{
    /// <summary>
    /// Custom attribute that, when applied on a class, automatically implements
    /// the <see cref="IReaderWriterSynchronized"/> interface. A new <see cref="ReaderWriterLockSlim"/>
    /// object is created upon each instantiation of the target class.
    /// </summary>
    [Serializable]
    [CompositionAspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
    [IntroduceInterface( typeof(IReaderWriterSynchronized) )]
    [ProvideAspectRole( StandardRoles.Threading )]
    public sealed class ReaderWriterSynchronizedAttribute : InstanceLevelAspect, IReaderWriterSynchronized
    {
        [NonSerialized] private ReaderWriterLockWrapper @lock;

        public override object CreateInstance( AdviceArgs aspectArgs )
        {
            ReaderWriterSynchronizedAttribute instance = new ReaderWriterSynchronizedAttribute
                                                             {
                                                                 @lock = new ReaderWriterLockWrapper( aspectArgs.Instance )
                                                             };
            return instance;
        }

        public ReaderWriterLockWrapper Lock
        {
            get { return this.@lock; }
        }
    }
}