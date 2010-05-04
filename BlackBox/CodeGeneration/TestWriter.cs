using System.IO;

namespace BlackBox.CodeGeneration
{
    public class TestWriter
    {
        private readonly StringWriter _stringWriter;
        private readonly RecordingXmlReader _recordingReader;
        private readonly IFile _fileAccess;
        private string _filename;

        public TestWriter() : this(new RecordingXmlReader(), new FileAdapter())
        {
        }       

        public TestWriter(RecordingXmlReader reader, IFile fileAccess)
        {
            _recordingReader = reader;
            _fileAccess = fileAccess;
            _stringWriter = new StringWriter();            
        }

        private bool _isFirstTestMethod = true;

        public void WriteTestMethod(string path)
        {
            _recordingReader.LoadRecording(path);
            
            if(_isFirstTestMethod)
            {
                WriteStartClass();
                _isFirstTestMethod = false;
            }            

            _stringWriter.WriteLine("\t\t[TestMethod]");
            _stringWriter.WriteLine("\t\tpublic void {0}()", Path.GetFileNameWithoutExtension(path));
            _stringWriter.WriteLine("\t\t{");
            _stringWriter.WriteLine("\t\t\tRun(@\"{0}\");", path);
            _stringWriter.WriteLine("\t\t}");
            _stringWriter.WriteLine();
        }

        private void WriteStartClass()
        {
            string method = _recordingReader.GetMethodName();
            string testFixtureName = CreateNameOfTestFixture(method);
            _filename = testFixtureName + ".cs";

            _stringWriter.WriteLine("using BlackBox;");
            _stringWriter.WriteLine("using BlackBox.Testing;");
            _stringWriter.WriteLine("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            _stringWriter.WriteLine();

            _stringWriter.WriteLine("namespace CharacterizationTests");
            _stringWriter.WriteLine("{");
            _stringWriter.WriteLine("\t[TestClass]");
            _stringWriter.WriteLine("\tpublic partial class {0} : CharacterizationTest", testFixtureName);
            _stringWriter.WriteLine("\t{");

            DeclareInputParameters();
            DecelareOutputParameters();

            _stringWriter.WriteLine("\t\tprivate {0} expected;", _recordingReader.GetTypeOfReturnValue());
            _stringWriter.WriteLine("\t\tprivate {0} actual;", _recordingReader.GetTypeOfReturnValue());
            _stringWriter.WriteLine("\t\tprivate {0} target;", _recordingReader.GetTypeRecordingWasMadeOn());
            _stringWriter.WriteLine();

            WriteRunMethod();
            WriteSetupMethod();            
        }

        private void DecelareOutputParameters()
        {
            foreach (var outputParameter in _recordingReader.GetOutputParametersMetadata())
            {
                _stringWriter.WriteLine("\t\tprivate {0} {1}Output;", outputParameter.TypeName, outputParameter.Name);
            }
            _stringWriter.WriteLine();
        }

        private void DeclareInputParameters()
        {
            foreach (var inputParameter in _recordingReader.GetInputParametersMetadata())
            {
                _stringWriter.WriteLine("\t\tprivate {0} {1}Input;", inputParameter.TypeName, inputParameter.Name);
            }
            _stringWriter.WriteLine();
        }

        private void WriteRunMethod()
        {
            _stringWriter.WriteLine("\t\tprivate void Run(string filename)");
            _stringWriter.WriteLine("\t\t{");

            _stringWriter.WriteLine("\t\t\tLoadRecording(filename);");
            _stringWriter.WriteLine("\t\t\ttarget = new {0}();", _recordingReader.GetTypeRecordingWasMadeOn());
            _stringWriter.WriteLine();

            string parameterList = WriteInputParameters();
            WriteOutputParameters();

            _stringWriter.WriteLine("\t\t\texpected = ({0})GetReturnValue();", _recordingReader.GetTypeOfReturnValue());
            _stringWriter.WriteLine("\t\t\tactual = target.{0}({1});", _recordingReader.GetMethodName(), parameterList);
            _stringWriter.WriteLine();

            var parameters = _recordingReader.GetInputParametersMetadata();
            parameters.ForEach(p => _stringWriter.WriteLine("\t\t\tCompareObjects({0}Input, {0}Output);", p.Name));
            
            _stringWriter.WriteLine("\t\t\tCompareObjects(expected, actual);");

            _stringWriter.WriteLine("\t\t}");
            _stringWriter.WriteLine();
        }

        private void WriteSetupMethod()
        {
            _stringWriter.WriteLine("\t\t[TestInitialize]");
            _stringWriter.WriteLine("\t\tpublic void Setup()");
            _stringWriter.WriteLine("\t\t{");
            _stringWriter.WriteLine("\t\t\tInitialize();");
            _stringWriter.WriteLine("\t\t}");
            _stringWriter.WriteLine();
        }

        private string WriteInputParameters()
        {
            string parameterList = "";

            var parameters = _recordingReader.GetInputParametersMetadata();
            for(int i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                parameterList += parameter.Name;
                if (i < parameters.Count - 1) parameterList += ", ";
                _stringWriter.WriteLine("\t\t\t{0}Input = ({1})GetInputParameterValue(\"{0}\");", parameter.Name, parameter.TypeName);
            }
            _stringWriter.WriteLine();
            return parameterList;
        }

        private void WriteOutputParameters()
        {
            var outputParameters = _recordingReader.GetOutputParametersMetadata();
            for (int i = 0; i < outputParameters.Count; i++)
            {
                var parameter = outputParameters[i];
                _stringWriter.WriteLine("\t\t\t{0}Output = ({1})GetOutputParameterValue(\"{0}\");", parameter.Name, parameter.TypeName);
            }
            _stringWriter.WriteLine();
        }

        private string CreateNameOfTestFixture(string method)
        {
            method = method.Replace("(", " ");
            method = method.Replace(",", "");
            method = method.Replace(")", "");
            method = method.Replace(".", "");
            method = method.Replace(" ", "_");
            return method;
        }

        public void SaveTest(string outputDirectory)
        {
            _stringWriter.WriteLine("\t}");
            _stringWriter.WriteLine("}");

            string path = Path.Combine(outputDirectory, _filename);
            _fileAccess.Save(_stringWriter.ToString(), path);            
        }
    }
}
