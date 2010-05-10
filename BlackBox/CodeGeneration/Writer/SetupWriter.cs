using System.Text;

namespace BlackBox.CodeGeneration.Writer
{
    internal class SetupWriter
    {
        private readonly StringBuilder _output;

        public SetupWriter(StringBuilder output)
        {
            _output = output;
        }

        public void WriteSetupMethod()
        {
            _output.AppendFormatLine("\t\t[{0}]", Configuration.TestFlavour.SetupAttribute);
            _output.AppendLine("\t\tpublic void Setup()");
            _output.AppendLine("\t\t{");
            _output.AppendLine("\t\t\tInitialize();");
            _output.AppendLine("\t\t}");
            _output.AppendLine();
        }

        public void WriteConsturctor(string testFixtureName)
        {
            _output.AppendFormatLine("\t\tpublic {0}()", testFixtureName);
            _output.AppendLine("\t\t{");
            _output.AppendLine("\t\t\tInitialize();");
            _output.AppendLine("\t\t}");
            _output.AppendLine();
        }

    }
}
