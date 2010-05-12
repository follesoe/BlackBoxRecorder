using BlackBox.CodeGeneration;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.CodeGeneration
{
    public class NUnitTestFlavourTest : TestsWithFlavour
    {
        [Fact]
        public void Generated_test_includes_correct_namespace()
        {
            generatedCode.ShouldContain("using NUnit.Framework;");
        }

        [Fact]
        public void Test_class_should_generate_one_test_method_for_each_recording()
        {
            generatedCode.ShouldContain("[Test]", 2);
        }

        [Fact]
        public void Test_class_should_generate_setup_method()
        {
            generatedCode.ShouldContain("[SetUp]");
            generatedCode.ShouldContain("Initialize();");
        }

        public override TestFlavour GetFlavour()
        {
            return TestFlavour.CreateNUnit();
        }
    }
}