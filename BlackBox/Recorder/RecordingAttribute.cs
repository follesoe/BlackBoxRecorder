using System;
using PostSharp.Laos;

namespace BlackBox.Recorder
{
    [Serializable]
    public class RecordingAttribute : OnMethodBoundaryAspect 
    {
        public override void OnEntry(MethodExecutionEventArgs eventArgs)
        {
            Guid callGuid = Guid.NewGuid();
            eventArgs.MethodExecutionTag = callGuid;
            var recording = new MethodRecording(eventArgs.Method, eventArgs.Instance, eventArgs.GetReadOnlyArgumentArray());
            RecordingServices.Recorder.RecordEntry(callGuid, recording);
        }

        public override void OnExit(MethodExecutionEventArgs eventArgs)
        {            
            RecordingServices.Recorder.RecordExit((Guid)eventArgs.MethodExecutionTag, eventArgs.ReturnValue);
        }
    }
}