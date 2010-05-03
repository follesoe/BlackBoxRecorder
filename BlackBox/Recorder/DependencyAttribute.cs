using System;
using PostSharp.Laos;

namespace BlackBox.Recorder
{
    [Serializable]
    public class DependencyAttribute : OnMethodInvocationAspect
    {
        public override void OnInvocation(MethodInvocationEventArgs eventArgs)
        {
            eventArgs.ReturnValue = eventArgs.Method.Invoke(eventArgs.Instance, eventArgs.GetArgumentArray());
            if (RecordingStack.Count > 0)
            {
                Guid callGuid = RecordingStack.Peek();
                RecordingServices.Recorder.RecordDependency(callGuid, eventArgs.Instance, eventArgs.Method, eventArgs.ReturnValue);
            }
        }
    }
}
