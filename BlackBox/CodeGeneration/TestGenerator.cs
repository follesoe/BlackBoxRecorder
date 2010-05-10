using System.IO;
using System.Collections.Generic;
using BlackBox.CodeGeneration.Writer;

namespace BlackBox.CodeGeneration
{
    public class TestGenerator : IGenerateTests
    {
        private readonly Dictionary<string, TestWriter> _writers;

        public TestGenerator()
        {
            _writers = new Dictionary<string, TestWriter>();    
        }

        public void GenerateTest(string recordingPath)
        {
            string directory = Path.GetDirectoryName(recordingPath);
            
            if(!_writers.ContainsKey(directory))
            {
                _writers.Add(directory, new TestWriter());
            }

            _writers[directory].WriteTest(recordingPath);
            _writers[directory].SaveTest(directory);
        }
    }
}
