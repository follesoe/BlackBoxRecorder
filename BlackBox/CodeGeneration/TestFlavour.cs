namespace BlackBox.CodeGeneration
{
    public class TestFlavour
    {
        public string Namespace { get; private set;}
        public string TestAttribute { get; private set; }
        public string ClassAttribute { get; private set; }
        public string SetupAttribute { get; private set; }

        public bool ConstructorAsSetup()
        {
            return string.IsNullOrEmpty(SetupAttribute);
        }

        private TestFlavour()
        {
        }

        public static TestFlavour CreateXunit()
        {
            return new TestFlavour
                       {
                           Namespace = "Xunit",
                           TestAttribute = "Fact"
                       };
        }

        public static TestFlavour CreateMSTest()
        {
            return new TestFlavour
                       {
                           Namespace = "Microsoft.VisualStudio.TestTools.UnitTesting",
                           ClassAttribute = "TestClass",
                           TestAttribute = "TestMethod",
                           SetupAttribute = "TestInitialize"
                       };
        }

        public static TestFlavour CreateNUnit()
        {
            return new TestFlavour
                       {
                           Namespace = "NUnit.Framework",
                           ClassAttribute = "TestFixture",
                           TestAttribute = "Test",
                           SetupAttribute = "SetUp"
                       };
        }
    }
}