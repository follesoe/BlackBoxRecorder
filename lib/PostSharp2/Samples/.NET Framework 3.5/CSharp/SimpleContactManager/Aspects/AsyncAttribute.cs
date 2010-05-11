using System;
using System.Threading;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace ContactManager.Aspects
{
    [Serializable]
    [ProvideAspectRole( StandardRoles.Threading )]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, "UI")]
    public sealed class AsyncAttribute : MethodInterceptionAspect
    {
        public override void OnInvoke( MethodInterceptionArgs eventArgs )
        {
            ThreadPool.QueueUserWorkItem( delegate { eventArgs.Proceed(); } );
        }
    }
}