using BlackBox.CodeGeneration.Writer;
using BlackBox.Tests.Fakes;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.CodeGeneration
{
    public class VoidMethodTestWriterTest
    {
        [Fact]
        public void Test_class_should_contain_call_on_void_method()
        {
            generatedCode.ShouldContain("target.UpdateContact");    
        }

        [Fact]
        public void Test_class_should_not_generate_comparison_of_expected_and_actual()
        {
            generatedCode.ShouldNotContain("CompareObjects(expected, actual);");
        }

        [Fact]
        public void Test_class_should_not_generate_expected_and_actual_field()
        {
            generatedCode.ShouldNotContain("expected;");
            generatedCode.ShouldNotContain("expected =");
            generatedCode.ShouldNotContain("actual;");
            generatedCode.ShouldNotContain("actual =");
        }

        public VoidMethodTestWriterTest()
        {
            var saveRecordings = new SaveRecordingsToMemory();
            RecordingServices.RecordingSaver = saveRecordings;
            fileSystem = new CodeGenerationFileSystem();
            testWriter = new TestWriter(saveRecordings, fileSystem);

            we_have_generated_a_test_class();
        }

        private void we_have_generated_a_test_class()
        {
            var contact = new Contact("Jonas Follesø", "jonas@follesoe.no");
            
            var addressbook = new SimpleAddressBook();
            addressbook.UpdateContact(contact);

            testWriter.WriteTest("UpdateContact");
            testWriter.SaveTest("UpdateContact_Test");
            generatedCode = fileSystem.GeneratedCode;
        }

        private string generatedCode;
        private readonly CodeGenerationFileSystem fileSystem;
        private readonly TestWriter testWriter;
    }
}
