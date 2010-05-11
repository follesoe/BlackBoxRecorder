using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace Librarian.Framework
{
  
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Class, Inheritance = MulticastInheritance.Strict)]
    [RequirePostSharp("Librarian", "Librarian")]
    public class EntityAspect :  TypeLevelAspect
    {
        /// <summary>
        /// Validates a field of an entity class (the field itself, not its value, since 
        /// we are at compile-time!).
        /// </summary>
        /// <param name="field">The field to be validated.</param>
        /// <returns><b>true</b> if this field is valid, otherwise <b>false</b>.</returns>
        private static bool ValidateEntityField(FieldInfo field)
        {
            // Check that the field type is not an entity in itself (should use an EntityRef).
            if (typeof(BaseEntity).IsAssignableFrom(field.FieldType))
            {
                LibrarianMessageSource.Instance.Write(SeverityType.Error, "LF0001",
                                                       new object[] { field.DeclaringType.Name, field.Name, field.FieldType.Name });
                return false;
            }

            // Check that the field type is either a value type either is cloneable.
            if (!field.FieldType.IsValueType &&
                 !typeof(ICloneable).IsAssignableFrom(field.FieldType) &&
                 field.FieldType != typeof(string))
            {
                LibrarianMessageSource.Instance.Write(SeverityType.Error, "LF0002",
                                                       new object[] { field.DeclaringType.Name, field.Name, field.FieldType.Name });
                return false;
            }

            // Check that the field type is serializable.
            if (!field.FieldType.IsSerializable)
            {
                LibrarianMessageSource.Instance.Write(SeverityType.Error, "LF0003",
                                                       new object[] { field.DeclaringType.Name, field.Name, field.FieldType.Name });
                return false;
            }

            // The field is correct.
            return true;
        }

        public override bool CompileTimeValidate(Type type)
        {
            bool ok = true;

            // Ensure that the type is serializable.
            if (!type.IsSerializable)
            {
                LibrarianMessageSource.Instance.Write(SeverityType.Error, "LF0004",
                                                       new object[] { type.Name });
                ok = false;
            }

            // Check fields.
            foreach (
                FieldInfo field in
                    type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (field.DeclaringType == type)
                {
                    ok &= ValidateEntityField(field);
                }
            }

            return ok;
        }
    }

}
