using BlackBox.CodeGeneration;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.CodeGeneration
{
    public class MSTestFlavourTest : TestsWithFlavour
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

        public override TestFlavour GetFlavour()
        {
            return new MSTestFlavour();
        }
    }
}
