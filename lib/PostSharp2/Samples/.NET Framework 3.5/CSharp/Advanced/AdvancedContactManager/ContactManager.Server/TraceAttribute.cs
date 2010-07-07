using System;
using System.Diagnostics;
using System.Reflection;
using ContactManager.Server;
using PostSharp.Aspects;

[assembly: Trace]

namespace ContactManager.Server
{
    [Serializable]
    [Trace( AttributeExclude = true )]
    internal class TraceAttribute : OnMethodBoundaryAspect
    {
        private string method;

        public override void CompileTimeInitialize( MethodBase method, AspectInfo aspectInfo )
        {
            this.method = method.DeclaringType.Name + "." + method.Name;
        }

        public override void OnEntry( MethodExecutionArgs args )
        {
            Trace.TraceInformation( "Entering " + method );
            Trace.Indent();
        }

        public override void OnSuccess( MethodExecutionArgs args )
        {
            Trace.Unindent();
            Trace.TraceInformation( "Entering " + method );
        }

        public override void OnException( MethodExecutionArgs args )
        {
            Trace.Unindent();
            Trace.TraceInformation( "Entering " + method + " with exception: " + args.Exception.ToString() );
        }
    }
}