namespace BlackBox.CodeGeneration
{
    public abstract class TestFlavour
    {
        public string Namespace { get; protected set;}
        public string TestAttribute { get; protected set; }
        public string ClassAttribute { get; protected set; }
        public string SetupAttribute { get; protected set; }

        public bool ConstructorAsSetup()
        {
            return string.IsNullOrEmpty(SetupAttribute);
        }
    }
}