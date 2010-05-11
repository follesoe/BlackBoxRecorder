using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PostSharp.Laos;

namespace ContactManager
{
    class TraceAttribute : OnMethodBoundaryAspect
    {
        private string methodName;

        public override void CompileTimeInitialize(System.Reflection.MethodBase method)
        {
            methodName = method.ToString();
        }
        public override void OnEntry(MethodExecutionEventArgs eventArgs)
        {
            Trace.WriteLine( "Enetring" + methodName  );
            base.OnEntry(eventArgs);
        }

        public override void OnException(MethodExecutionEventArgs eventArgs)
        {
            base.OnException(eventArgs);
        }

        public override void OnSuccess(MethodExecutionEventArgs eventArgs)
        {
            base.OnSuccess(eventArgs);
        }
    }
}
