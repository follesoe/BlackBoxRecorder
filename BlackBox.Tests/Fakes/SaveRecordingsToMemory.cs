using System.Xml.Linq;
using System.Collections.Generic;

using BlackBox.Recorder;

namespace BlackBox.Tests.Fakes
{
    public class SaveRecordingsToMemory : RecordingXmlReader, ISaveRecordings
    {
        private readonly Stack<XDocument> _recordings;
        private readonly RecordingXmlWriter _xmlWriter;

        public SaveRecordingsToMemory()
        {
            _recordings = new Stack<XDocument>();
            _xmlWriter = new RecordingXmlWriter();
        }

        public void SaveMethodRecording(MethodRecording recording)
        {
            _recordings.Push(_xmlWriter.CreateXml(recording));
        }

        public override void LoadRecording(string path)
        {
            CurrentRecording = _recordings.Pop();
        }
    }
}
