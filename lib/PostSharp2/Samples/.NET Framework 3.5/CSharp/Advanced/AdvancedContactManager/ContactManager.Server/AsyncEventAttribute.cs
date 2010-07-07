using System.Threading;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Serialization;

namespace ContactManager.Server
{
    [EventInterceptionAspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
    [Trace( AttributeExclude = true )]
    public class AsyncEventAttribute : EventInterceptionAspect
    {
        public override void OnInvokeHandler( EventInterceptionArgs args )
        {
            ThreadPool.QueueUserWorkItem( state =>
                                              {
                                                  try
                                                  {
                                                      args.ProceedInvokeHandler();
                                                  }
                                                  catch
                                                  {
                                                      args.ProceedRemoveHandler();
                                                  }
                                              } );
        }
    }
}