using System;
using PostSharp.Laos;

namespace BlackBox.Recorder
{
    [Serializable]
    public class RecordingAttribute : OnMethodBoundaryAspect 
    {
        public override void OnEntry(MethodExecutionEventArgs eventArgs)
        {
            eventArgs.MethodExecutionTag = Guid.NewGuid();           
            RecordingServices.Recorder.RecordEntry(eventArgs.Instance, eventArgs.Method, eventArgs.GetReadOnlyArgumentArray());
        }

        public override void OnExit(MethodExecutionEventArgs eventArgs)
        {            
            RecordingServices.Recorder.RecordExit(eventArgs.Method, eventArgs.ReturnValue);
        }
    }
}