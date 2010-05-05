using BlackBox.CodeGeneration;
using BlackBox.Tests.Fakes;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.CodeGeneration
{
    public class MSTestFlavour
    {
        [Fact]
        public void Generated_test_includes_correct_namespace()
        {
            generatedCode.ShouldContain("using Microsoft.VisualStudio.TestTools.UnitTesting;");
        }

        [Fact]
        public void Test_class_should_generate_one_test_method_for_each_recording()
        {
            generatedCode.ShouldContain("[TestMethod]", 2);
        }

        [Fact]
        public void Test_class_should_generate_setup_method()
        {
            generatedCode.ShouldContain("[TestInitialize]");
            generatedCode.ShouldContain("Initialize();");
        }

        public MSTestFlavour()
        {
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
            testWriter.WriteTestMethod("foo");
            testWriter.WriteTestMethod("foo2");
            testWriter.SaveTest("bar");
            generatedCode = fileSystem.GeneratedCode;
        }

        private string generatedCode;
        private readonly CodeGenerationFileSystem fileSystem;
        private readonly TestWriter testWriter;
        private readonly SimpleMath math;
    }
}
