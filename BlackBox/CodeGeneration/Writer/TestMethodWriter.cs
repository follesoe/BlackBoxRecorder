using System.Collections.Generic;
using System.Text;

namespace BlackBox.CodeGeneration.Writer
{
    internal class TestMethodWriter
    {
        private readonly Dictionary<string, int> _usedMethodNames;
        private readonly RecordingXmlReader _recordingReader;
        private readonly StringBuilder _output;
       

        public TestMethodWriter(RecordingXmlReader reader, StringBuilder output)
        {
            _usedMethodNames = new Dictionary<string, int>();
            _recordingReader = reader;
            _output = output;
        }

        public void WriteTestMethod(string path)
        {            
            if (!string.IsNullOrEmpty(Configuration.TestFlavour.TestAttribute))
            {
                _output.AppendFormatLine("\t\t[{0}]", Configuration.TestFlavour.TestAttribute);
            }

            string recordingName = _recordingReader.GetRecordingName();
            if (!_usedMethodNames.ContainsKey(recordingName))
            {
                _usedMethodNames.Add(recordingName, 1);
            }
            else
            {
                int usedTimes = _usedMethodNames[recordingName];
                _usedMethodNames[recordingName] = usedTimes++;
                recordingName += "_" + usedTimes;
            }

            _output.AppendFormatLine("\t\tpublic void {0}()", recordingName);
            _output.AppendLine("\t\t{");
            _output.AppendFormatLine("\t\t\tRun(@\"{0}\");", path);
            _output.AppendLine("\t\t}");
            _output.AppendLine();
        }
    }
}
