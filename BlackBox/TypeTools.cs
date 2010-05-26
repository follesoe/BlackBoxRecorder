using System;
using System.Linq;
using System.Reflection;

namespace BlackBox
{
    public class TypeTools
    {
        // "Unwrap" means that if the root type is a generic collection,
        // then we return the generic type parameter
        public static Type UnwrapType(string qualifiedTypeName)
        {
            Type type = Type.GetType(qualifiedTypeName, false);
            if (type.IsGenericType)
                return type.GetGenericArguments().First();
            return type;
        }

        // Return the name of any public instance property
        // or a default string if the type does not contain
        // any such properties.
        public static string GetSomePublicPropertyName(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                       .Select(p => p.Name)
                       .DefaultIfEmpty("NoPublicInstancePropertiesAvailable")
                       .FirstOrDefault();
        }
    }
}
