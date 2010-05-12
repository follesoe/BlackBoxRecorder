using System;
using PostSharp.Aspects;

namespace BlackBox.Recorder
{
    [Serializable]
    public class RecordingAttribute : OnMethodBoundaryAspect 
    {
        public override void OnEntry(MethodExecutionArgs eventArgs)
        {
            if (Configuration.IsPlayback()) return;

            var callGuid = Guid.NewGuid();
            eventArgs.MethodExecutionTag = callGuid;
            RecordingServices.Recorder.RecordEntry(callGuid, eventArgs.Method, eventArgs.Instance, eventArgs.Arguments.ToArray());

            RecordingStack.Push(callGuid);
        }
        public override void OnExit(MethodExecutionArgs eventArgs)
        {
            if (Configuration.IsPlayback()) return;

            RecordingServices.Recorder.RecordExit((Guid)eventArgs.MethodExecutionTag, eventArgs.Arguments.ToArray(), eventArgs.ReturnValue);
            RecordingStack.Pop();
        }
    }
}