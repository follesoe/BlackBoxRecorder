using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Test.ObjectComparison;

namespace BlackBox.Testing
{
    public static class Extensions
    {
        public static string ToMismatchDetailsString(this IEnumerable<ObjectComparisonMismatch> mismatches)
        {
            var toStringBuilder = new StringBuilder(Environment.NewLine + Environment.NewLine);
            if(mismatches.Any())
                mismatches.ToList().ForEach(m => toStringBuilder.AppendLine(m.ToString()));
            return toStringBuilder.ToString();
        }

        public static string ToQualifiedName(this MemberInfo memberInfo)
        {
            if (memberInfo.DeclaringType == null)
                return memberInfo.Name;

            Type type = memberInfo.DeclaringType;
            return type.FullName.Replace(type.Namespace, "") + "." + memberInfo.Name;
        }

        public static IEnumerable<ObjectComparisonMismatch> Exclude(this IEnumerable<ObjectComparisonMismatch> mismatches,
                                                                    IEnumerable<string> mismatchesToExclude)
        {
            return mismatches.Where(m => !mismatchesToExclude.Contains(m.LeftObjectNode.QualifiedName));
        }

        public static IEnumerable<ObjectComparisonMismatch> Exclude(this IEnumerable<ObjectComparisonMismatch> mismatches,
                                                            List<MemberInfo> propertiesToIgnore)
        {
            var ignoreList = new List<ObjectComparisonMismatch>();
            foreach (var propertyToIgnore in propertiesToIgnore)
            {
                var potentialMismatches = mismatches.Where(m => m.LeftObjectNode.QualifiedName == propertyToIgnore.ToQualifiedName());
                foreach (var mismatch in potentialMismatches)
                {
                    if (mismatch.LeftObjectNode.Parent.Name == propertyToIgnore.DeclaringType.Name)
                        ignoreList.Add(mismatch);
                }
            }
            return mismatches.Where(m => !ignoreList.Contains(m));

            ////var ignoreList = 
            ////foreach (var mismatch in mismatches)
            ////{
            ////    var possiblePropertyMatches = propertiesToIgnore.Where(p => p.Name == mismatch.LeftObjectNode.Name);
            ////    if(possiblePropertyMatches.Any())
            ////    {
                    
            ////    }
            ////}
            ////var ignoreList = mismatches.Where(m => m.LeftObjectNode.Name)
            //////return mismatches.Where(m => !mismatchesToExclude.Contains(m.LeftObjectNode.QualifiedName));
        }

    }
}
