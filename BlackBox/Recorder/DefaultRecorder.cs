using System;
using System.Collections.Generic;
using System.Reflection;

namespace BlackBox.Recorder
{
    public class DefaultRecorder : IRecordMethodCalls   
    {
        public List<MethodRecording> MethodRecordings { get; private set; }

        private readonly Dictionary<Guid, MethodRecording> _notExited;

        public DefaultRecorder()
        {
            MethodRecordings = new List<MethodRecording>();
            _notExited = new Dictionary<Guid, MethodRecording>();
        }

        public void RecordEntry(Guid callGuid, MethodBase methodBase, object instance, object[] inputParameters)
        {
            var recording = new MethodRecording(methodBase, instance, inputParameters);
            _notExited.Add(callGuid, recording);
        }

        public void RecordExit(Guid callGuid, object[] outputParameters, object returnValue)
        {
            MethodRecording recording = _notExited[callGuid];

            recording.AddReturnValues(outputParameters, returnValue);

            MethodRecordings.Add(recording);
            _notExited.Remove(callGuid);

            RecordingServices.RecordingSaver.SaveMethodRecording(recording);
        }

        public void RecordDependency(Guid callGuid, object instance, MethodInfo method, object returnValue)
        {           
            MethodRecording recording = _notExited[callGuid];
            recording.DependencyRecordings.Add(returnValue);
        }

        public void ClearRecordings()
        {
            MethodRecordings.Clear();
        }
    }
}