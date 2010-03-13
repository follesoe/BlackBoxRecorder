using System.IO;

namespace BlackBox.CodeGeneration
{
    public class TestWriter
    {
        private readonly StringWriter _stringWriter;
        private readonly RecordingXmlReader _recordingReader;
        private string _filename;

        public TestWriter(string path)
        {
            _stringWriter = new StringWriter();
            _recordingReader = new RecordingXmlReader();            
            WriteStartClass(path);
        }

        private void WriteStartClass(string path)
        {
            _recordingReader.LoadRecording(path);

            var directory = new DirectoryInfo(path);
            string type = directory.Parent.Parent.Name;
            string method = directory.Parent.Name;
            string testFixtureName = CreateNameOfTestFixture(type, method);
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

            foreach (var parameter in _recordingReader.GetParametersMetadata())
            {
                _stringWriter.WriteLine("\t\tprivate {0} {1};", parameter.TypeName, parameter.Name);
            }
            _stringWriter.WriteLine("\t\tprivate {0} expected;", _recordingReader.GetTypeOfReturnValue());
            _stringWriter.WriteLine("\t\tprivate {0} actual;", _recordingReader.GetTypeOfReturnValue());
            _stringWriter.WriteLine("\t\tprivate {0} target;", _recordingReader.GetTypeRecordingWasMadeOn());
            _stringWriter.WriteLine();

            WriteRunMethod();
        }

        private void WriteRunMethod()
        {
            _stringWriter.WriteLine("\t\tprivate void Run(string filename)");
            _stringWriter.WriteLine("\t\t{");

            _stringWriter.WriteLine("\t\t\tLoadRecording(filename);");
            _stringWriter.WriteLine("\t\t\ttarget = new {0}();", _recordingReader.GetTypeRecordingWasMadeOn());

            string parameterList = "";
            var parameters = _recordingReader.GetParametersMetadata();
            for(int i = 0; i < parameters.Count; ++i)
            {
                var parameter = parameters[0];
                parameterList += parameter.Name;
                if (i < parameters.Count - 1) parameterList += ", ";
                _stringWriter.WriteLine("\t\t\t{0} = ({1})GetParameterValue(\"{0}\");", parameter.Name, parameter.TypeName);                
            }
            _stringWriter.WriteLine("\t\t\texpected = ({0})GetReturnValue();", _recordingReader.GetTypeOfReturnValue());
            _stringWriter.WriteLine("\t\t\tactual = target.{0}({1});", _recordingReader.GetMethodName(), parameterList);
            _stringWriter.WriteLine("\t\t\tCompareObjects(expected, actual);");

            _stringWriter.WriteLine("\t\t}");
            _stringWriter.WriteLine();
        }

        public void WriteTestMethod(string path)
        {
            _recordingReader.LoadRecording(path);
            
            _stringWriter.WriteLine("\t\t[TestMethod]");
            _stringWriter.WriteLine("\t\tpublic void {0}()", Path.GetFileNameWithoutExtension(path));
            _stringWriter.WriteLine("\t\t{");
            _stringWriter.WriteLine("\t\t\tRun(@\"{0}\");", path);
            _stringWriter.WriteLine("\t\t}");
            _stringWriter.WriteLine();
        }

        private string CreateNameOfTestFixture(string type, string method)
        {
            type = type.Replace(".", "");            
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

            using(var sw = new StreamWriter(Path.Combine(outputDirectory, _filename)))
            {
                sw.WriteLine(_stringWriter.ToString());                
            }
        }
    }
}
