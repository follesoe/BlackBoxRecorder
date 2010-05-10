using System.Text;

namespace BlackBox.CodeGeneration.Writer
{
    internal class RunMethodWriter
    {
        private readonly ParameterWriter _parameterWriter;
        private readonly RecordingXmlReader _recordingReader;
        private readonly StringBuilder _sb;

        public RunMethodWriter(RecordingXmlReader reader, StringBuilder output)
        {
            _parameterWriter = new ParameterWriter(reader, output);
            _recordingReader = reader;
            _sb = output;
        }

        public void WriteRunMethod()
        {
            string recordingWasMadeOn = _recordingReader.GetTypeRecordingWasMadeOn();
            _sb.AppendFormatLine("\t\tprivate void Run(string filename)");
            _sb.AppendLine("\t\t{");

            _sb.AppendFormatLine("\t\t\tLoadRecording(filename);");

            if (!_recordingReader.GetMethodIsStatic())
            {
                _sb.AppendFormatLine("\t\t\ttarget = new {0}();", recordingWasMadeOn);
                _sb.AppendLine();
            }

            string parameterList =  _parameterWriter.WriteInputParameters();
            _parameterWriter.WriteOutputParameters();

            _sb.AppendFormatLine("\t\t\texpected = ({0})GetReturnValue();", _recordingReader.GetTypeOfReturnValue());

            if (_recordingReader.GetMethodIsStatic())
            {
                _sb.AppendFormatLine("\t\t\tactual = {0}.{1}({2});", recordingWasMadeOn, _recordingReader.GetMethodName(), parameterList);
            }
            else
            {
                _sb.AppendFormatLine("\t\t\tactual = target.{0}({1});", _recordingReader.GetMethodName(), parameterList);
            }

            _sb.AppendLine();

            var parameters = _recordingReader.GetInputParametersMetadata();
            parameters.ForEach(p => _sb.AppendFormatLine("\t\t\tCompareObjects({0}Input, {0}Output);", p.Name));

            _sb.AppendFormatLine("\t\t\tCompareObjects(expected, actual);");

            _sb.AppendLine("\t\t}");
            _sb.AppendLine();
        }
    }
}
