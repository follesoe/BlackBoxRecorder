using System.Text;

namespace BlackBox.CodeGeneration.Writer
{
    internal class ParameterWriter
    {
        private readonly RecordingXmlReader _reader;
        private readonly StringBuilder _output;

        public ParameterWriter(RecordingXmlReader reader, StringBuilder output)
        {
            _reader = reader;
            _output = output;
        }

        public string WriteInputParameters()
        {
            var parameterList = "";

            var parameters = _reader.GetInputParametersMetadata();
            for (var i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                parameterList += parameter.Name + "Input";
                if (i < parameters.Count - 1) parameterList += ", ";
                _output.AppendFormatLine("\t\t\t{0}Input = ({1})GetInputParameterValue(\"{0}\");", parameter.Name, parameter.TypeName);
            }
            _output.AppendLine();
            return parameterList;
        }

        public void WriteOutputParameters()
        {
            var outputParameters = _reader.GetOutputParametersMetadata();
            foreach (var parameter in outputParameters)
            {
                _output.AppendFormatLine("\t\t\t{0}Output = ({1})GetOutputParameterValue(\"{0}\");", parameter.Name, parameter.TypeName);
            }
            _output.AppendLine();
        }

    }
}
