namespace BlackBox.CodeGeneration
{
    public class MSTestFlavour : TestFlavour
    {
        public MSTestFlavour()
        {
            Namespace = "Microsoft.VisualStudio.TestTools.UnitTesting";
            TestAttribute = "TestMethod";
            SetupAttribute = "TestInitialize";
        }
    }
}
