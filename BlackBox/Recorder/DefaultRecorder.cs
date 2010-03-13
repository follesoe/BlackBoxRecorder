using System;
using System.Collections.Generic;

using OX.Copyable;

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

        public void RecordEntry(Guid callGuid, MethodRecording recording)
        {
            _notExited.Add(callGuid, recording);
        }

        public void RecordExit(Guid callGuid, object returnValue)
        {
            MethodRecording recording = _notExited[callGuid];
            recording.ReturnValue = returnValue.Copy();
            MethodRecordings.Add(recording);
            _notExited.Remove(callGuid);
            
            RecordingServices.RecordingSaver.SaveMethodRecording(recording);
        }

        public void ClearRecordings()
        {
            MethodRecordings.Clear();
        }
    }
}