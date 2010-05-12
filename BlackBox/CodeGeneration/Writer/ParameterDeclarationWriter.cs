using System.Text;

namespace BlackBox.CodeGeneration.Writer
{
    internal class ParameterDeclarationWriter
    {
        private readonly RecordingXmlReader _reader;
        private readonly StringBuilder _output;

        public ParameterDeclarationWriter(RecordingXmlReader reader, StringBuilder output)
        {
            _reader = reader;
            _output = output;
        }

        public void WriteInputParametersDeclaration()
        {
            foreach (var inputParameter in _reader.GetInputParametersMetadata())
            {
                _output.AppendFormatLine("\t\tprivate {0} {1}Input;", inputParameter.TypeName, inputParameter.Name);
            }
        }

        public void WriteOutputParametersDeclaration()
        {
            foreach (var outputParameter in _reader.GetOutputParametersMetadata())
            {
                _output.AppendFormatLine("\t\tprivate {0} {1}Output;", outputParameter.TypeName, outputParameter.Name);
            }
            _output.AppendLine();

        }
    }
}
