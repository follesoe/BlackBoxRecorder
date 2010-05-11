using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace Dependencies.Aspects
{
    [Serializable]
    [AspectTypeDependency(AspectDependencyAction.Commute, typeof(CacheAttribute))]
    class TraceAttribute : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine("Entering {0}.{1}", args.Method.DeclaringType, args.Method);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine("Leaving {0}.{1}", args.Method.DeclaringType, args.Method);
        }
    }
}
