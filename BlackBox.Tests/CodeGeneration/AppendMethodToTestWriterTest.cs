using BlackBox.CodeGeneration;
using BlackBox.Tests.Fakes;

using Xunit;

namespace BlackBox.Tests.CodeGeneration
{
    public class AppendMethodToTestWriterTest : BDD<AppendMethodToTestWriterTest>
    {
        [Fact]
        public void Can_add_new_recording_after_saving_the_test()
        {
            Given.we_have_generated_a_test_class();

            generatedCode.ShouldHaveEqualNumberOf("{", "}");
        }

        public AppendMethodToTestWriterTest()
        {
            var saveRecordings = new SaveRecordingsToMemory();
            RecordingServices.RecordingSaver = saveRecordings;
            fileSystem = new CodeGenerationFileSystem();
            testWriter = new TestWriter(saveRecordings, fileSystem);
        }

        private void we_have_generated_a_test_class()
        {
            SimpleMath.AddStatic(5, 5);                        
            testWriter.WriteTestMethod("AddStatic1");
            testWriter.SaveTest("bar");

            SimpleMath.AddStatic(10, 10);
            testWriter.WriteTestMethod("AddStatic2");
            testWriter.SaveTest("bar");
            
            generatedCode = fileSystem.GeneratedCode;
        }

        private string generatedCode;
        private readonly CodeGenerationFileSystem fileSystem;
        private readonly TestWriter testWriter;
    }
}
