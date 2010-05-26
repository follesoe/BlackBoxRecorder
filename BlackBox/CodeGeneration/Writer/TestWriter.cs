using System.IO;
using System.Text;

namespace BlackBox.CodeGeneration.Writer
{
    public class TestWriter
    {
        private const int CharacterToRemove = 9;
        private readonly RecordingXmlReader _reader;
        private readonly IFile _fileAccess;

        private readonly StringBuilder _output;
        private readonly BodyWriter _bodyWriter;
        private readonly TestMethodWriter _methodWriter;
        private readonly ComparisonConfigurationWriter _configurationWriter;
        private bool _isSaved;

        public TestWriter() : this(new RecordingXmlReader(), new FileAdapter())
        {
        }       

        public TestWriter(RecordingXmlReader reader, IFile fileAccess)
        {
            _reader = reader;
            _fileAccess = fileAccess;

            _output = new StringBuilder();
            _bodyWriter = new BodyWriter(reader, _output);
            _methodWriter = new TestMethodWriter(reader, _output);
            _configurationWriter = new ComparisonConfigurationWriter(reader, _output);
        }

        public void WriteTest(string path)
        {
            _reader.LoadRecording(path);

            _bodyWriter.WriteBody();

            RemoveClosingBrackets();

            _configurationWriter.WriteConfigurationMethod(path);

            _methodWriter.WriteTestMethod(path);
        }

        private void RemoveClosingBrackets()
        {
            if (_isSaved)
            {
                _output.Remove(_output.Length - CharacterToRemove, CharacterToRemove);
            }
        }

        public void SaveTest(string outputDirectory)
        {
            _output.AppendLine("\t}");
            _output.AppendLine("}");

            string path = Path.Combine(outputDirectory, _bodyWriter.TestFixtureName + ".cs");
            string outputString = _output.ToString();
            _fileAccess.Save(outputString, path);
            _isSaved = true;
        }
    }
}
