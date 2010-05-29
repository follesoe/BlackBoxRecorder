using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackBox.Tests.Testing.ObjectComparison
{
    public static class EnumerableExtensions
    {
        public static bool None<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }
    }
}
