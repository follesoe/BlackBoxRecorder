using System;
using System.Collections.Generic;
using System.Text;
using PostSharp.Aspects;

namespace Trace
{
    [Serializable]
    public sealed class QuickTraceAttribute : OnMethodBoundaryAspect
    {
        private string enteringMessage, leavingMessage;

        public override void CompileTimeInitialize(System.Reflection.MethodBase method, AspectInfo aspectInfo)
        {
            string methodName = method.DeclaringType.FullName + "." + method.Name;
            this.enteringMessage = "Entering " + methodName;
            this.leavingMessage = "Leaving " + methodName;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            System.Diagnostics.Trace.WriteLine( this.enteringMessage);
            System.Diagnostics.Trace.Indent();
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            System.Diagnostics.Trace.Unindent();
            System.Diagnostics.Trace.WriteLine( this.leavingMessage );
        }

        public override void OnException(MethodExecutionArgs args)
        {
            System.Diagnostics.Trace.Unindent();
            System.Diagnostics.Trace.WriteLine(this.leavingMessage + " with exception: " +
                args.Exception.Message + Environment.NewLine + args.Exception.ToString());
        }
    }
}
