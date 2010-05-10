using BlackBox.CodeGeneration;
using BlackBox.CodeGeneration.Writer;
using BlackBox.Tests.Fakes;

namespace BlackBox.Tests.CodeGeneration
{
    public abstract class TestsWithFlavour
    {
        public abstract TestFlavour GetFlavour();
        
        protected TestsWithFlavour()
        {
            Configuration.TestFlavour = GetFlavour();

            var saveRecordings = new SaveRecordingsToMemory();
            RecordingServices.RecordingSaver = saveRecordings;
            
            fileSystem = new CodeGenerationFileSystem();
            testWriter = new TestWriter(saveRecordings, fileSystem);

            math = new SimpleMath();
            we_have_generated_a_test_class();
        }

        private void we_have_generated_a_test_class()
        {
            math.Add(5, 5);
            math.Add(10, 10);
            testWriter.WriteTest("foo");
            testWriter.WriteTest("foo2");
            testWriter.SaveTest("bar");
            generatedCode = fileSystem.GeneratedCode;
        }

        protected string generatedCode;
        private readonly CodeGenerationFileSystem fileSystem;
        private readonly TestWriter testWriter;
        private readonly SimpleMath math;
    }
}
