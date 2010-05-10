using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlackBox.CodeGeneration.Writer
{
    public class TestWriter
    {
        private readonly RecordingXmlReader _reader;
        private readonly IFile _fileAccess;

        private readonly StringBuilder _output;
        private readonly BodyWriter _bodyWriter;
        private readonly TestMethodWriter _methodWriter;
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
        }

        public void WriteTest(string path)
        {
            _reader.LoadRecording(path);

            _bodyWriter.WriteBody();
            
            if(_isSaved)
            {
                _output.Remove(_output.Length - 8, 8);
            }

            _methodWriter.WriteTestMethod(path);
        }

        public void SaveTest(string outputDirectory)
        {
            _output.AppendLine("\t}");
            _output.AppendLine("}");

            string path = Path.Combine(outputDirectory, _bodyWriter.TestFixtureName + ".cs");
            _fileAccess.Save(_output.ToString(), path);
            _isSaved = true;
        }
    }
}
