using System.Text;
using System.Collections.Generic;

namespace BlackBox.CodeGeneration.Writer
{
    internal class ComparisonConfigurationWriter
    {
        private readonly RecordingXmlReader _recordingReader;
        private readonly StringBuilder _output;

        public ComparisonConfigurationWriter(RecordingXmlReader reader, StringBuilder output)
        {
            _recordingReader = reader;
            _output = output;
        }

        public void WriteConfigurationMethod(string path)
        {
            _output.AppendLine("\t\tprotected override void ConfigureComparison(string filename)");
            _output.AppendLine("\t\t{");
            _output.AppendFormatLine("\t\t\tif(filename.EndsWith(\"{0}.xml\"))", _recordingReader.GetRecordingName());
            _output.AppendLine("\t\t\t{");
            _output.AppendLine("\t\t\t\t//IgnoreOnType(TODO);");
            _output.AppendLine("\t\t\t}");
            _output.AppendLine("\t\t}");
            _output.AppendLine();
        }
    }
}
