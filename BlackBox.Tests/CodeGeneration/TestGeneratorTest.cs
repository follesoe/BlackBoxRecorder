using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackBox.CodeGeneration;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.CodeGeneration
{
    public class TestGeneratorTest
    {
        [Fact]
        public void Finds_all_recordings_in_sub_folder()
        {
            generator.GenerateTests("TestInputFiles", "TestInputFiles");
            generator.RecordingFileNames.Count.ShouldEqual(4);
        }

        public TestGeneratorTest()
        {
            generator = new TestGenerator();    
        }

        private readonly TestGenerator generator;
    }
}
