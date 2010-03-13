using System;
using PostSharp.Laos;

namespace BlackBox.Recorder
{
    [Serializable]
    public class RecordingAttribute : OnMethodBoundaryAspect 
    {
        public override void OnEntry(MethodExecutionEventArgs eventArgs)
        {
            string recordingName = RecordingServices.RecordingNamer.GetNameForRecording(eventArgs.Method);
            RecordingServices.Recorder.RecordEntry(recordingName, eventArgs.Instance, eventArgs.Method, eventArgs.GetReadOnlyArgumentArray());
        }

        public override void OnExit(MethodExecutionEventArgs eventArgs)
        {            
            RecordingServices.Recorder.RecordExit(eventArgs.Method, eventArgs.ReturnValue);
        }
    }
}