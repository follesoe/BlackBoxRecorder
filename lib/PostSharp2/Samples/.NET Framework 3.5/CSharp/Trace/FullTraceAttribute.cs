using System;
using System.Collections.Generic;
using System.Text;
using PostSharp.Aspects;

namespace Trace
{
    [Serializable]
    public sealed class FullTraceAttribute : OnMethodBoundaryAspect
    {
        private MethodFormatStrings methodFormatStrings;

        public override void CompileTimeInitialize(System.Reflection.MethodBase method, AspectInfo aspectInfo)
        {
            this.methodFormatStrings = Formatter.GetMethodFormatStrings( method );
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            System.Diagnostics.Trace.WriteLine( "Entering " + 
                this.methodFormatStrings.Format( args.Instance, args.Method, args.Arguments.ToArray()));

            System.Diagnostics.Trace.Indent();
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            System.Diagnostics.Trace.Unindent();

            System.Diagnostics.Trace.WriteLine("Leaving " +
                this.methodFormatStrings.Format(args.Instance, args.Method, args.Arguments.ToArray()));

        }

        public override void OnException(MethodExecutionArgs args)
        {
            System.Diagnostics.Trace.Unindent();
            System.Diagnostics.Trace.WriteLine("Leaving " +
                this.methodFormatStrings.Format(args.Instance, args.Method, args.Arguments.ToArray())
                + " with exception: " +
                args.Exception.Message + Environment.NewLine + args.Exception.ToString());

        }
    }
}
