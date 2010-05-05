namespace BlackBox.CodeGeneration
{
    public class XunitFlavour : TestFlavour
    {
        public XunitFlavour()
        {
            Namespace = "Xunit";
            TestAttribute = "Fact";
        }
    }
}
