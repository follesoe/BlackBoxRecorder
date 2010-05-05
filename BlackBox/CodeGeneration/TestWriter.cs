using System.IO;
using System.Text;
using System.Collections.Generic;

namespace BlackBox.CodeGeneration
{
    public class TestWriter
    {
        private readonly StringBuilder _sb;
        private readonly RecordingXmlReader _recordingReader;
        private readonly IFile _fileAccess;
               
        private bool _isSaved;
        private bool _isFirstTestMethod = true;
        private string _filename;
        private string _testFixtureName;
        private readonly Dictionary<string, int> _usedMethodNames;

        public TestWriter() : this(new RecordingXmlReader(), new FileAdapter())
        {
        }       

        public TestWriter(RecordingXmlReader reader, IFile fileAccess)
        {
            _recordingReader = reader;
            _fileAccess = fileAccess;
            _usedMethodNames = new Dictionary<string, int>();
            _sb = new StringBuilder();            
        }

        public void WriteTestMethod(string path)
        {
            _recordingReader.LoadRecording(path);
            
            if(_isFirstTestMethod)
            {
                WriteStartClass();
                _isFirstTestMethod = false;
            }    
        
            var methodBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(Configuration.TestFlavour.TestAttribute))
            {
                methodBuilder.AppendFormatLine("\t\t[{0}]", Configuration.TestFlavour.TestAttribute);
            }

            string recordingName = _recordingReader.GetRecordingName();
            if(!_usedMethodNames.ContainsKey(recordingName))
            {
                _usedMethodNames.Add(recordingName, 1);
            }
            else
            {
                int usedTimes = _usedMethodNames[recordingName];
                _usedMethodNames[recordingName] = usedTimes++;
                recordingName += "_" + usedTimes;
            }

            methodBuilder.AppendFormatLine("\t\tpublic void {0}()", recordingName);
            methodBuilder.AppendLine("\t\t{");
            methodBuilder.AppendFormatLine("\t\t\tRun(@\"{0}\");", path);
            methodBuilder.AppendLine("\t\t}");
            methodBuilder.AppendLine();

            if (_isSaved)
            {
                _sb.Remove(_sb.Length - 8, 8);
            }
            _sb.Append(methodBuilder.ToString());
        }

        private void WriteStartClass()
        {
            _testFixtureName = CreateNameOfTestFixture();
            _filename = _testFixtureName + ".cs";

            _sb.AppendFormatLine("using BlackBox;");
            _sb.AppendFormatLine("using BlackBox.Testing;");
            _sb.AppendFormatLine("using {0};", Configuration.TestFlavour.Namespace);
            _sb.AppendLine();

            _sb.AppendFormatLine("namespace CharacterizationTests");
            _sb.AppendLine("{");
            _sb.AppendFormatLine("\t" + Configuration.TestFlavour.ClassAttribute);
            _sb.AppendFormatLine("\tpublic partial class {0} : CharacterizationTest", _testFixtureName);
            _sb.AppendLine("\t{");

            DeclareInputParameters();
            DecelareOutputParameters();

            _sb.AppendFormatLine("\t\tprivate {0} expected;", _recordingReader.GetTypeOfReturnValue());
            _sb.AppendFormatLine("\t\tprivate {0} actual;", _recordingReader.GetTypeOfReturnValue());

            if (!_recordingReader.GetMethodIsStatic())
            {
                _sb.AppendFormatLine("\t\tprivate {0} target;", _recordingReader.GetTypeRecordingWasMadeOn());
            }

            _sb.AppendLine();

            WriteRunMethod();

            if(Configuration.TestFlavour.ConstructorAsSetup())
            {
                WriteConsturctor();
            }
            else
            {
                WriteSetupMethod();
            }
        }
 
        private void DecelareOutputParameters()
        {
            foreach (var outputParameter in _recordingReader.GetOutputParametersMetadata())
            {
                _sb.AppendFormatLine("\t\tprivate {0} {1}Output;", outputParameter.TypeName, outputParameter.Name);
            }
            _sb.AppendLine();
        }

        private void DeclareInputParameters()
        {
            foreach (var inputParameter in _recordingReader.GetInputParametersMetadata())
            {
                _sb.AppendFormatLine("\t\tprivate {0} {1}Input;", inputParameter.TypeName, inputParameter.Name);
            }
            _sb.AppendLine();
        }

        private void WriteRunMethod()
        {
            string recordingWasMadeOn = _recordingReader.GetTypeRecordingWasMadeOn();
            _sb.AppendFormatLine("\t\tprivate void Run(string filename)");
            _sb.AppendLine("\t\t{");

            _sb.AppendFormatLine("\t\t\tLoadRecording(filename);");

            if(!_recordingReader.GetMethodIsStatic())
            {
                _sb.AppendFormatLine("\t\t\ttarget = new {0}();", recordingWasMadeOn);
                _sb.AppendLine();
            }                       

            string parameterList = WriteInputParameters();
            WriteOutputParameters();

            _sb.AppendFormatLine("\t\t\texpected = ({0})GetReturnValue();", _recordingReader.GetTypeOfReturnValue());

            if(_recordingReader.GetMethodIsStatic())
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

        private void WriteSetupMethod()
        {
            _sb.AppendFormatLine("\t\t[{0}]", Configuration.TestFlavour.SetupAttribute);
            _sb.AppendLine("\t\tpublic void Setup()");
            _sb.AppendLine("\t\t{");
            _sb.AppendLine("\t\t\tInitialize();");
            _sb.AppendLine("\t\t}");
            _sb.AppendLine();
        }

        private void WriteConsturctor()
        {
            _sb.AppendFormatLine("\t\tpublic {0}()", _testFixtureName);
            _sb.AppendLine("\t\t{");
            _sb.AppendLine("\t\t\tInitialize();");
            _sb.AppendLine("\t\t}");
            _sb.AppendLine();
        }

        private string WriteInputParameters()
        {
            string parameterList = "";

            var parameters = _recordingReader.GetInputParametersMetadata();
            for(int i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                parameterList += parameter.Name + "Input";
                if (i < parameters.Count - 1) parameterList += ", ";
                _sb.AppendFormatLine("\t\t\t{0}Input = ({1})GetInputParameterValue(\"{0}\");", parameter.Name, parameter.TypeName);
            }
            _sb.AppendLine();
            return parameterList;
        }

        private void WriteOutputParameters()
        {
            var outputParameters = _recordingReader.GetOutputParametersMetadata();
            for (int i = 0; i < outputParameters.Count; i++)
            {
                var parameter = outputParameters[i];
                _sb.AppendFormatLine("\t\t\t{0}Output = ({1})GetOutputParameterValue(\"{0}\");", parameter.Name, parameter.TypeName);
            }
            _sb.AppendLine();
        }

        private string CreateNameOfTestFixture()
        {
            string methodName = _recordingReader.GetMethodName();
            
            foreach(var parameter in _recordingReader.GetInputParametersMetadata())
            {
                methodName += "_" + parameter.Name;
            }

            return methodName + "_Tests";
        }

        public void SaveTest(string outputDirectory)
        {
            _sb.AppendLine("\t}");
            _sb.AppendLine("}");

            string path = Path.Combine(outputDirectory, _filename);
            _fileAccess.Save(_sb.ToString(), path);
            _isSaved = true;
        }
    }
}