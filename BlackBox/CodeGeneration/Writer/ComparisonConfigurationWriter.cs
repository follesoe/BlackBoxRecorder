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

            string qualifiedName = ConstructAssemblyQualifiedNameExample();

            _output.AppendLine("\t\tprotected override void ConfigureComparison(string filename)");
            _output.AppendLine("\t\t{");
            _output.AppendLine("\t\t\t//// Use the filename of the test to setup different");
            _output.AppendLine("\t\t\t//// comparison configurations for each test.");
            _output.AppendFormatLine("\t\t\t//if(filename.EndsWith(\"{0}.xml\"))", _recordingReader.GetRecordingName());
            _output.AppendLine("\t\t\t//{");
            _output.AppendLine("\t\t\t//    // Use IgnoreOnType to exclude a property from the comparison for all objects of that type.");
            _output.AppendFormatLine("\t\t\t//    IgnoreOnType({0});", ConstructTypePropertySelectorExample(qualifiedName));
            _output.AppendLine("\t\t\t//");
            _output.AppendLine("\t\t\t//    // Use Ignore to exclude a property from the comparison for a specific instance.");
            _output.AppendFormatLine("\t\t\t//    Ignore({0});", ConstructObjectPropertySelectorExample(qualifiedName));
            _output.AppendLine("\t\t\t//}");
            _output.AppendLine("\t\t}");
            _output.AppendLine();

            _configurationSectionIsWritten = true;
        }

        private string ConstructAssemblyQualifiedNameExample()
        {
            if (!_recordingReader.IsVoidMethod())
                return _recordingReader.GetAssemblyQualifiedNameOfReturnValue();

            bool hasInputParameters = _recordingReader.GetInputParameters().Any();
            if (hasInputParameters)
                return _recordingReader.GetInputParameters().First().Type.AssemblyQualifiedName;

            return typeof(string).AssemblyQualifiedName;
        }

        private static string ConstructObjectPropertySelectorExample(string assemblyQualifiedName)
        {
            string prefix = "expected";
            if (TypeTools.IsGenericType(assemblyQualifiedName))
                prefix += ".First()";
            return prefix + ", " + ConstructTypePropertySelectorExample(assemblyQualifiedName);
        }

        private static string ConstructTypePropertySelectorExample(string assemblyQualifiedName)
        {
            Type unwrappedType = TypeTools.UnwrapType(assemblyQualifiedName);
            string fullName = unwrappedType.FullName;
            return string.Format("({0} {1}) => {1}.{2}",
                                 fullName,
                                 fullName.ToLowerInvariant().First(),
                                 TypeTools.GetSomePublicPropertyName(unwrappedType));
        }
    }
}
