using System;
using Xunit;

namespace BlackBox.Tests
{
    public static class TestExtensions
    {
        public static void ShouldContain(this string text, string checkFor, int occurences)
        {
            var result = text.Split(new [] { checkFor }, StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(occurences, result.Length - 1);
        }
    }
}
