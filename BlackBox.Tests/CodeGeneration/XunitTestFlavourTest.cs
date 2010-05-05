using BlackBox.CodeGeneration;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.CodeGeneration
{
    public class XunitTestFlavourTest : TestsWithFlavour
    {
        [Fact]
        public void Generated_test_includes_correct_namespace()
        {
            generatedCode.ShouldContain("using Xunit;");
        }

        [Fact]
        public void Test_class_should_generate_one_test_method_for_each_recording()
        {
            generatedCode.ShouldContain("[Fact]", 2);
        }

        [Fact]
        public void Test_class_should_generate_unique_test_names()
        {
            generatedCode.ShouldContain("public void Add_a_b()");
            generatedCode.ShouldContain("public void Add_a_b_2()");
        }

        [Fact]
        public void Test_class_should_generate_constructor_for_setup()
        {
            generatedCode.ShouldContain("public Add_a_b_Tests()");
        }

        public override TestFlavour GetFlavour()
        {
            return TestFlavour.CreateXunit();
        }
    }
}
