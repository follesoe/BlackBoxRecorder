using System;

namespace BlackBox.Recorder
{
    public interface IRecordMethodCalls
    {
        void RecordEntry(Guid callGuid, MethodRecording recording);
        void RecordExit(Guid callGuid, object returnValue);
    }
}