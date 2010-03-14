using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;

namespace BlackBox.Recorder
{
    public class SaveRecordingToDisk : ISaveRecordings
    {
        private readonly IFile _file;
        private readonly RecordingXmlWriter _xmlWriter;
        private readonly HashSet<string> _savedFiles;
        
        public SaveRecordingToDisk(IFile file)
        {
            _file = file;
            _xmlWriter = new RecordingXmlWriter();
            _savedFiles = new HashSet<string>();
        }

        public void SaveMethodRecording(MethodRecording recording)
        {
            string typeDirectory = CreateTypeDirectory(recording);
            string methodDirectory = CreateMethodDirectory(typeDirectory, recording);
            string recordingPath = CreateRecordingPath(methodDirectory, recording);
            SaveRecording(recordingPath, recording);
        }

        private string CreateTypeDirectory(MethodRecording recording)
        {
            string typeDirectory = Path.Combine("Recordings", recording.CalledOnType.FullName);
            if (!_file.DirectoryExists(typeDirectory))
            {
                _file.CreateDirectory(typeDirectory);
            }
            return typeDirectory;
        }

        private string CreateMethodDirectory(string typeDirectory, MethodRecording recording)
        {
            string methodDirectory = Path.Combine(typeDirectory, recording.MethodName);
            if (!_file.DirectoryExists(methodDirectory))
            {
                _file.CreateDirectory(methodDirectory);
            }
            return methodDirectory;
        }

        private string CreateRecordingPath(string methodDirectory, MethodRecording recording)
        {
            string recordingPathWithoutExtension = Path.Combine(methodDirectory, recording.RecordingName);
            string recordingPath = recordingPathWithoutExtension + ".xml";

            int counter = 1;            
            while (_savedFiles.Contains(recordingPath))
            {
                counter++;
                recordingPath = recordingPathWithoutExtension + "_" + counter + ".xml";
            }
            _savedFiles.Add(recordingPath);
            
            return recordingPath;            
        }

        private void SaveRecording(string path, MethodRecording recording)
        {
            XDocument xml = _xmlWriter.CreateXml(recording);
            _file.Save(xml, path);
        }
    }
}
