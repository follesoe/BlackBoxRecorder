using System.Reflection;
using System.Collections.Generic;

using OX.Copyable;

namespace BlackBox.Recorder
{
    public class DefaultRecorder : IRecordMethodCalls    
    {
        public List<MethodRecording> MethodRecordings { get; private set; }

        private readonly Dictionary<MethodBase, MethodRecording> _notExited;

        public DefaultRecorder()
        {
            MethodRecordings = new List<MethodRecording>();
            _notExited = new Dictionary<MethodBase, MethodRecording>();
        }

        public void RecordEntry(string recordingName, object instance, MethodBase method, object[] parameters)
        {
            var recording = new MethodRecording(recordingName, method, instance, parameters);
            _notExited.Add(method, recording);
        }

        public void RecordExit(MethodBase method, object returnValue)
        {
            MethodRecording recording = _notExited[method];
            recording.ReturnValue = returnValue.Copy();
            MethodRecordings.Add(recording);
            _notExited.Remove(method);
            
            RecordingServices.RecordingSaver.SaveMethodRecording(recording);
        }

        public void ClearRecordings()
        {
            MethodRecordings.Clear();
        }
    }
}