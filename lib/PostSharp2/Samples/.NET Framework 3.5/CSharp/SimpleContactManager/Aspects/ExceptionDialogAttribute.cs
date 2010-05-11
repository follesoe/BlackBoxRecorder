using System;
using System.Windows;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace ContactManager.Aspects
{
    [Serializable]
    [ProvideAspectRole( StandardRoles.ExceptionHandling )]
    [AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Threading )]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, "UI")]
    public sealed class ExceptionDialogAttribute : OnExceptionAspect
    {
        public override void OnException( MethodExecutionArgs eventArgs )
        {
            // Compose the error message. We could do something more complex.
            string message = eventArgs.Exception.Message;

            MessageBoxHelper.Display( eventArgs.Instance as DependencyObject, message );

            // Ignore the exception.
            eventArgs.FlowBehavior = FlowBehavior.Continue;
        }
    }
}