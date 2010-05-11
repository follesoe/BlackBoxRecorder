using System.Text;

namespace BlackBox.CodeGeneration.Writer
{
    internal class RunMethodWriter
    {
        private readonly ParameterWriter _parameterWriter;
        private readonly RecordingXmlReader _reader;
        private readonly StringBuilder _output;

        public RunMethodWriter(RecordingXmlReader reader, StringBuilder output)
        {
            _parameterWriter = new ParameterWriter(reader, output);
            _reader = reader;
            _output = output;
        }

        public void WriteRunMethod()
        {
            string recordingWasMadeOn = _reader.GetTypeRecordingWasMadeOn();
            _output.AppendFormatLine("\t\tprivate void Run(string filename)");
            _output.AppendLine("\t\t{");

            _output.AppendFormatLine("\t\t\tLoadRecording(filename);");

            if (!_reader.IsStaticMethod())
            {
                _output.AppendFormatLine("\t\t\ttarget = new {0}();", recordingWasMadeOn);
                _output.AppendLine();
            }

            string parameterList =  _parameterWriter.WriteInputParameters();
            _parameterWriter.WriteOutputParameters();
            
            if(_reader.IsVoidMethod())
            {
                _output.AppendFormatLine("\t\t\ttarget.{0}({1});", _reader.GetMethodName(), parameterList);
            }
            else
            {
                _output.AppendFormatLine("\t\t\texpected = ({0})GetReturnValue();", _reader.GetTypeOfReturnValue());
                if (_reader.IsStaticMethod())
                {
                    _output.AppendFormatLine("\t\t\tactual = {0}.{1}({2});", recordingWasMadeOn, _reader.GetMethodName(), parameterList);
                }
                else
                {
                    _output.AppendFormatLine("\t\t\tactual = target.{0}({1});", _reader.GetMethodName(), parameterList);
                }                
            }

            _output.AppendLine();

            var parameters = _reader.GetInputParametersMetadata();
            parameters.ForEach(p => _output.AppendFormatLine("\t\t\tCompareObjects({0}Input, {0}Output);", p.Name));

            if (!_reader.IsVoidMethod())
            {
                _output.AppendFormatLine("\t\t\tCompareObjects(expected, actual);");
            }

            _output.AppendLine("\t\t}");
            _output.AppendLine();
        }
    }
}
