using System.Text;

namespace BlackBox
{
    public static class StringBuilderExtension
    {
        public static void AppendFormatLine(this StringBuilder sb, string format, params object[] args)
        {
            sb.AppendFormat(format, args);
            sb.AppendLine();
        }
    }
}
