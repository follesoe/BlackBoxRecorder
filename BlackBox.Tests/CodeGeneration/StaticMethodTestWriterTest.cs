using BlackBox.CodeGeneration.Writer;
using BlackBox.Tests.Fakes;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.CodeGeneration
{
    public class StaticMethodTestWriterTest
    {
        [Fact]
        public void Test_class_should_generate_setup_method()
        {       
            generatedCode.ShouldContain("actual = BlackBox.Tests.Fakes.SimpleMath.AddStatic(aInput, bInput);");
        }

        [Fact]
        public void Test_class_should_not_generate_target_field()
        {
            generatedCode.ShouldNotContain("private BlackBox.Tests.Fakes.SimpleMath target;");
        }

        [Fact]
        public void Test_class_should_not_instantiate_target()
        {
            generatedCode.ShouldNotContain("target = new BlackBox.Tests.Fakes.SimpleMath();");
        }

        public StaticMethodTestWriterTest()
        {
            var saveRecordings = new SaveRecordingsToMemory();
            RecordingServices.RecordingSaver = saveRecordings;
            fileSystem = new CodeGenerationFileSystem();
            testWriter = new TestWriter(saveRecordings, fileSystem);

            we_have_generated_a_test_class();
        }

        private void we_have_generated_a_test_class()
        {
            SimpleMath.AddStatic(5, 5);
            SimpleMath.AddStatic(10, 10);
            testWriter.WriteTest("foo");
            testWriter.WriteTest("foo2");
            testWriter.SaveTest("bar");
            generatedCode = fileSystem.GeneratedCode;
        }

        private string generatedCode;
        private readonly CodeGenerationFileSystem fileSystem;
        private readonly TestWriter testWriter;
    }
}
