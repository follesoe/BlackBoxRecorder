using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Serialization;

namespace ContactManager.Framework
{
    /// <summary>
    /// Exposes the <see cref="Finalized"/> event.
    /// </summary>
    /// <seealso cref="ObservableFinalizeAspect"/>
    public interface IObservableFinalize
    {
        /// <summary>
        /// Event raised when the object is finalized (i.e. when its destructor is called).
        /// </summary>
        event EventHandler Finalized;
    }

    /// <summary>
    /// Implementation of <see cref="IObservableFinalize"/>
    /// to be composed into objects, eventually using an aspect.
    /// </summary>
    [IntroduceInterface( typeof(IObservableFinalize) )]
    [AspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
    public class ObservableFinalizeAspect : InstanceLevelAspect, IObservableFinalize
    {
        private EventHandler collected;

        public override object CreateInstance(AdviceArgs aspectArgs)
        {
            object o = base.CreateInstance(aspectArgs);
            GC.SuppressFinalize(o);
            return o;
        }


        /// <inheritdoc />
        public event EventHandler Finalized
        {
            add
            {
                if ( collected == null )
                    GC.ReRegisterForFinalize( this );

                collected += value;
            }

            remove
            {
                collected -= value;

                if ( collected == null )
                    GC.SuppressFinalize( this );
            }
        }

        /// <inheritdoc />
        ~ObservableFinalizeAspect()
        {
            if ( this.collected != null )
            {
                this.collected( this.Instance, EventArgs.Empty );
            }
        }
    }
}