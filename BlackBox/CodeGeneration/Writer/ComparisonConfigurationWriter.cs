using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace BlackBox.CodeGeneration.Writer
{
    internal class ComparisonConfigurationWriter
    {
        private readonly RecordingXmlReader _recordingReader;
        private readonly StringBuilder _output;
        private bool _configurationSectionIsWritten;

        public ComparisonConfigurationWriter(RecordingXmlReader reader, StringBuilder output)
        {
            _recordingReader = reader;
            _output = output;
        }

        public void WriteConfigurationMethod(string path)
        {
            if (_configurationSectionIsWritten)
                return;

            _output.AppendLine("\t\tprotected override void ConfigureComparison(string filename)");
            _output.AppendLine("\t\t{");
            _output.AppendFormatLine("\t\t\t//if(filename.EndsWith(\"{0}.xml\"))", _recordingReader.GetRecordingName());
            _output.AppendLine("\t\t\t//{");
            _output.AppendFormatLine("\t\t\t//    IgnoreOnType({0});", ConstructPropertySelectorExample());
            _output.AppendLine("\t\t\t//}");
            _output.AppendLine("\t\t}");
            _output.AppendLine();

            _configurationSectionIsWritten = true;
        }

        private string ConstructPropertySelectorExample()
        {
            Type unwrappedType = TypeTools.UnwrapType(_recordingReader.GetAssemblyQualifiedNameOfReturnValue());
            string fullName = unwrappedType.FullName;
            return string.Format("({0} {1}) => {1}.{2}",
                                 fullName,
                                 fullName.ToLowerInvariant().First(),
                                 TypeTools.GetSomePublicPropertyName(unwrappedType));
        }
    }
}
