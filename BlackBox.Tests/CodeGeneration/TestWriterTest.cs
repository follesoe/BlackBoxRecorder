using BlackBox.CodeGeneration;

using Xunit;
using Xunit.Extensions;
using BlackBox.Tests.Fakes;

namespace BlackBox.Tests.CodeGeneration
{
    public class TestWriterTest : BDD<TestWriterTest>
    {
        [Fact]
        public void Test_class_has_declared_input_parameter_variables()
        {
            generatedCode.ShouldContain("private System.Int32 aInput;");
            generatedCode.ShouldContain("private System.Int32 bInput;");
        }

        [Fact]
        public void Test_class_has_declared_output_parameter_variables()
        {
            generatedCode.ShouldContain("private System.Int32 aOutput;");
            generatedCode.ShouldContain("private System.Int32 bOutput;");
        }

        [Fact]
        public void Test_class_has_assigned_input_parameter_variables()
        {
            generatedCode.ShouldContain("aInput = (System.Int32)GetInputParameterValue(\"a\");");
            generatedCode.ShouldContain("bInput = (System.Int32)GetInputParameterValue(\"b\");");
        }

        [Fact]
        public void Test_class_has_assigned_output_parameter_variables()
        {           
            generatedCode.ShouldContain("aOutput = (System.Int32)GetOutputParameterValue(\"a\");");
            generatedCode.ShouldContain("bOutput = (System.Int32)GetOutputParameterValue(\"b\");");
        }

        [Fact]
        public void Test_class_has_assigned_actual_value()
        {
            generatedCode.ShouldContain("actual = target.Add(aInput, bInput);");
        }

        [Fact]
        public void Test_class_should_compare_input_to_output_parameters()
        {
            generatedCode.ShouldContain("CompareObjects(aInput, aOutput);");
            generatedCode.ShouldContain("CompareObjects(bInput, bOutput);");
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

        public TestWriterTest()
        {
            var saveRecordings = new SaveRecordingsToMemory();
            RecordingServices.RecordingSaver = saveRecordings;
            fileSystem = new CodeGenerationFileSystem();
            testWriter = new TestWriter(saveRecordings, fileSystem);

            math = new SimpleMath();
            Given.we_have_generated_a_test_class();
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