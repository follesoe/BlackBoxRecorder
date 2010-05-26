using BlackBox.CodeGeneration.Writer;
using Xunit;
using Xunit.Extensions;

using BlackBox.Tests.Fakes;

namespace BlackBox.Tests.CodeGeneration
{
    public class TestWriterTest
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
        public void Test_class_should_contain_a_call_to_the_comparison_configuration()
        {
            generatedCode.ShouldContain("ConfigureComparison(filename);");
        }

        [Fact]
        public void Test_class_should_compare_input_to_output_parameters()
        {
            generatedCode.ShouldContain("CompareObjects(aInput, aOutput);");
            generatedCode.ShouldContain("CompareObjects(bInput, bOutput);");
        }

        [Fact]
        public void Test_class_should_contain_an_example_of_how_to_configure_the_object_comparison()
        {
            generatedCode.ShouldContain("protected override void ConfigureComparison(string filename)");
            generatedCode.ShouldContain("if(filename.EndsWith(");
            generatedCode.ShouldContain("IgnoreOnType((System.Int32 s) => s");
        }

        [Fact]
        public void Test_class_should_be_indented_properly()
        {
            generatedCode.ShouldContain("\t\tprivate void Run(string filename)");
        }

        public TestWriterTest()
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
            testWriter.WriteTest("foo");
            testWriter.WriteTest("foo2");
            testWriter.SaveTest("bar");
            generatedCode = fileSystem.GeneratedCode;
        }

        private string generatedCode;
        private readonly CodeGenerationFileSystem fileSystem;
        private readonly TestWriter testWriter;
        private readonly SimpleMath math;
    }
}