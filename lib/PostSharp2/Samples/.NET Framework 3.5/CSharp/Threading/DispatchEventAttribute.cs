using System;
using System.Windows.Threading;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Serialization;

namespace Threading
{
    [EventInterceptionAspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
    public sealed class DispatchEventAttribute : EventInterceptionAspect
    {
        public override void OnInvokeHandler( EventInterceptionArgs args )
        {
            DispatcherObject dispatcherObject = args.Handler.Target as DispatcherObject;

            if ( dispatcherObject == null || dispatcherObject.CheckAccess() )
            {
                args.ProceedInvokeHandler();
            }
            else
            {
                // We have to dispatch synchronously to avoid the object to be changed
                // before the time the event is raised and the time it is processed.
                dispatcherObject.Dispatcher.Invoke( DispatcherPriority.Normal, new Action( args.ProceedInvokeHandler ) );
            }
        }
    }
}