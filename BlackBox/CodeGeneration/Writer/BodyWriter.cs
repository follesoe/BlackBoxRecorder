using System.Text;

namespace BlackBox.CodeGeneration.Writer
{
    internal class BodyWriter
    {
        private readonly ParameterDeclarationWriter _parameterWriter;
        private readonly RunMethodWriter _runMethodWriter;
        private readonly SetupWriter _setupWriter;
        private readonly RecordingXmlReader _reader;
        private readonly StringBuilder _output;

        private bool _bodyIsWritten;

        public string TestFixtureName { get; private set; }

        public BodyWriter(RecordingXmlReader reader, StringBuilder output)
        {
            _reader = reader;
            _output = output;
            _parameterWriter = new ParameterDeclarationWriter(reader, output);
            _runMethodWriter = new RunMethodWriter(reader, output);
            _setupWriter = new SetupWriter(output);
        }

        public void WriteBody()
        {
            if (_bodyIsWritten) return;

            CreateNameOfTestFixture();
            
            _output.AppendFormatLine("using BlackBox.Testing;");
            _output.AppendFormatLine("using {0};", Configuration.TestFlavour.Namespace);
            _output.AppendLine();

            _output.AppendFormatLine("namespace CharacterizationTests");
            _output.AppendLine("{");

            if (!string.IsNullOrEmpty(Configuration.TestFlavour.ClassAttribute))
            {
                _output.AppendFormatLine("\t[{0}]", Configuration.TestFlavour.ClassAttribute);
            }

            _output.AppendFormatLine("\tpublic partial class {0} : CharacterizationTest", TestFixtureName);
            _output.AppendLine("\t{");

            _parameterWriter.WriteInputParametersDeclaration();
            _parameterWriter.WriteOutputParametersDeclaration();

            if (!_reader.IsVoidMethod())
            {
                _output.AppendFormatLine("\t\tprivate {0} expected;", _reader.GetTypeOfReturnValue());
                _output.AppendFormatLine("\t\tprivate {0} actual;", _reader.GetTypeOfReturnValue());
            }

            if (!_reader.IsStaticMethod())
            {
                _output.AppendFormatLine("\t\tprivate {0} target;", _reader.GetTypeRecordingWasMadeOn());
            }

            _output.AppendLine();

            _runMethodWriter.WriteRunMethod();

            if (Configuration.TestFlavour.ConstructorAsSetup())
            {
                _setupWriter.WriteConsturctor(TestFixtureName);
            }
            else
            {
                _setupWriter.WriteSetupMethod();
            }

            _bodyIsWritten = true;
        }

        private void CreateNameOfTestFixture()
        {            
            string methodName = _reader.GetMethodName();

            foreach (var parameter in _reader.GetInputParametersMetadata())
            {
                methodName += "_" + parameter.Name;
            }

            TestFixtureName = methodName + "_Tests";
        }
    }
}
