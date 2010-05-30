namespace BlackBox.CodeGeneration
{
    public class TestFlavour
    {
        public string Namespace { get; private set;}
        public string TestAttribute { get; private set; }
        public string FixtureAttribute { get; private set; }
        public string SetupAttribute { get; private set; }
        public string CategoryAttribute { get; set; }

        public bool ConstructorAsSetup()
        {
            return string.IsNullOrEmpty(SetupAttribute);
        }

        public bool UseFixtureAttribute()
        {
            return !string.IsNullOrEmpty(FixtureAttribute);
        }

        public bool UseCategoryAttribute()
        {
            return !string.IsNullOrEmpty(CategoryAttribute);
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
                           FixtureAttribute = "TestClass",
                           TestAttribute = "TestMethod",
                           SetupAttribute = "TestInitialize"
                       };
        }

        public static TestFlavour CreateNUnit()
        {
            return new TestFlavour
                       {
                           Namespace = "NUnit.Framework",
                           FixtureAttribute = "TestFixture",
                           CategoryAttribute = "Category",
                           TestAttribute = "Test",
                           SetupAttribute = "SetUp"
                       };
        }
    }
}