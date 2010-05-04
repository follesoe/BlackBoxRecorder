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

        public static void ShouldHaveEqualNumberOf(this string text, string checkFor1, string checkFor2)
        {
            var result1 = text.Split(new[] { checkFor1 }, StringSplitOptions.RemoveEmptyEntries);
            var result2 = text.Split(new[] { checkFor2 }, StringSplitOptions.RemoveEmptyEntries);

            Assert.True(result1.Length == result2.Length,
                        string.Format("Could not find same number of occurances of \"{0}\" ({1}) as of \"{2}\" ({3})", 
                            checkFor1, result1.Length, checkFor2, result2.Length));
        }

        public static void ShouldNotContain(this string text, string checkFor)
        {
            int index = text.IndexOf(checkFor);
            Assert.True(index == -1, "Found string \"" + checkFor + "\" in string. This was not expected.");
        }
    }
}
