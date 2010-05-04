using System;
using System.Linq;
using System.Reflection;
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

        public static string GetMethodNameWithoutTilde(this MethodBase method)
        {
            string name = method.Name;
            return name.StartsWith("~") ? name.Substring(1) : name;
        }

        public static string GetMethodNameWithParameters(this MethodBase method)
        {
            string name = method.Name + "(";
            
            var parameters = method.GetParameters();
            
            for(int i = 0; i < parameters.Length; ++i)
            {
                name += parameters[i].Name;
                if(i < parameters.Length - 1)
                {
                    name += ", ";
                }
            }
            name += ")";
            return name;
        }
    }
}
