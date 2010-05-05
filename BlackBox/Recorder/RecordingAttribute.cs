using System;
using PostSharp.Laos;

namespace BlackBox.Recorder
{
    [Serializable]
    public class RecordingAttribute : OnMethodBoundaryAspect 
    {
        public override void OnEntry(MethodExecutionEventArgs eventArgs)
        {
            if (Configuration.IsPlayback()) return;

            Guid callGuid = Guid.NewGuid();
            eventArgs.MethodExecutionTag = callGuid;
            RecordingServices.Recorder.RecordEntry(callGuid, eventArgs.Method, eventArgs.Instance, eventArgs.GetReadOnlyArgumentArray());
            
            RecordingStack.Push(callGuid);
        }

        public override void OnExit(MethodExecutionEventArgs eventArgs)
        {
            if (Configuration.IsPlayback()) return;

            RecordingServices.Recorder.RecordExit((Guid)eventArgs.MethodExecutionTag, eventArgs.GetReadOnlyArgumentArray(), eventArgs.ReturnValue);
            RecordingStack.Pop();
        }
    }
}