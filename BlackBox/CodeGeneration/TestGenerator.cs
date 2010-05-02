using System.IO;
using System.Collections.Generic;

namespace BlackBox.CodeGeneration
{
    public class TestGenerator
    {
        public List<string> RecordingFileNames { get; private set; }

        private readonly Dictionary<string, TestWriter> _writers;           
        private string _outputDirectory;
        
        public TestGenerator()
        {
            _writers = new Dictionary<string, TestWriter>();
            RecordingFileNames = new List<string>();
        }

        public void GenerateTests(string inputDirectory, string outputDirectory)
        {
            _outputDirectory = outputDirectory;
            FindRecordingFileNames(inputDirectory);
            RecordingFileNames.ForEach(GenerateTestMethod);
            SaveTests();
        }

        private void GenerateTestMethod(string inputDirectory)
        {
            var directory = new DirectoryInfo(inputDirectory);
            string type = directory.Parent.Parent.Name;
            string method = directory.Parent.Name;
            string key = type + method;

            TestWriter writer;
            if (!_writers.ContainsKey(key))
            {
                writer = new TestWriter();
                _writers.Add(key, writer);
                
            }
            else
            {
                writer = _writers[key];
            }

            writer.WriteTestMethod(inputDirectory);
        }

        private void SaveTests()
        {
            foreach(var writer in _writers.Values)
            {
                writer.SaveTest(_outputDirectory);
            }
        }

        private void FindRecordingFileNames(string rootPath)
        {
            RecordingFileNames.Clear();

            foreach(var directory in Directory.GetDirectories(rootPath))
                FindRecordingFileNames(directory);

            var files = Directory.GetFiles(rootPath, "*.xml");

            foreach(var recording in files)
            {
                RecordingFileNames.Add(recording);
            }
        }
    }
}
