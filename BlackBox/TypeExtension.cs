using System;
using System.Linq;
using System.Text;

namespace BlackBox
{
    public static class TypeExtension
    {
        private const char GenericSpecialChar = '`';
        private const string GenericSeparator = ", ";

        public static string GetCleanName(this Type t)
        {
            string name = t.Name;
            if (t.IsGenericType)
            {
                name = name.Remove(name.IndexOf(GenericSpecialChar));
            }
            return name;
        }

        public static string GetCodeDefinition(this Type t)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}.{1}", t.Namespace, t.GetCleanName());
            if (t.IsGenericType)
            {
                var names = from ga in t.GetGenericArguments()
                            select GetCodeDefinition(ga);

                sb.Append("<");
                sb.Append(string.Join(GenericSeparator, names.ToArray()));
                sb.Append(">");
            }
            return sb.ToString();
        }
    }
}
