using System;
using System.Xml.Linq;
using System.Collections.Generic;

using BlackBox.Recorder;
using BlackBox.CodeGeneration;

using Xunit;
using Xunit.Extensions;
using BlackBox.Tests.Fakes;

namespace BlackBox.Tests.CodeGeneration
{
    public class TestWriterTest : BDD<TestWriterTest>
    {
        [Fact]
        public void Test_class_has_declared_input_parameter_variables()
        {
            generatedCode.ShouldContain("private System.Int32 aInput;");
            generatedCode.ShouldContain("private System.Int32 bInput;");
        }

        [Fact]
        public void Test_class_has_declared_output_parameter_variables()
        {
            generatedCode.ShouldContain("private System.Int32 aOutput;");
            generatedCode.ShouldContain("private System.Int32 bOutput;");
        }

        [Fact]
        public void Test_class_has_assigned_input_parameter_variables()
        {
            generatedCode.ShouldContain("aInput = (System.Int32)GetInputParameterValue(\"a\");");
            generatedCode.ShouldContain("bInput = (System.Int32)GetInputParameterValue(\"b\");");
        }

        [Fact]
        public void Test_class_has_assigned_output_parameter_variables()
        {           
            generatedCode.ShouldContain("aOutput = (System.Int32)GetOutputParameterValue(\"a\");");
            generatedCode.ShouldContain("bOutput = (System.Int32)GetOutputParameterValue(\"b\");");
        }

        [Fact]
        public void Test_class_should_compare_input_to_output_parameters()
        {
            generatedCode.ShouldContain("CompareObjects(aInput, aOutput);");
            generatedCode.ShouldContain("CompareObjects(bInput, bOutput);");
        }

        [Fact]
        public void Test_class_should_generate_one_test_method_for_each_recording()
        {
            generatedCode.ShouldContain("[TestMethod]", 2);
        }

        [Fact]
        public void Test_class_should_generate_setup_method()
        {
            generatedCode.ShouldContain("[TestInitialize]");
            generatedCode.ShouldContain("Initialize();");
        }

        public TestWriterTest()
        {
            var saveRecordings = new SaveRecordingsToMemory();
            RecordingServices.RecordingSaver = saveRecordings;
            fileSystem = new CodeGenerationFileSystem();
            testWriter = new TestWriter(saveRecordings, fileSystem);

            math = new SimpleMath();
            Given.we_have_generated_a_test_class();
        }

        private void we_have_generated_a_test_class()
        {
            math.Add(5, 5);
            math.Add(10, 10);
            testWriter.WriteTestMethod("foo");
            testWriter.WriteTestMethod("foo2");
            testWriter.SaveTest("bar");
            generatedCode = fileSystem.GeneratedCode;
        }

        private string generatedCode;
        private CodeGenerationFileSystem fileSystem;
        private TestWriter testWriter;
        private SimpleMath math;
    }

    public class CodeGenerationFileSystem : IFile
    {
        public string GeneratedCode;
        public string CodeSavedToPath;

        public void Save(string testClass, string path)
        {
            GeneratedCode = testClass;
            CodeSavedToPath = path;
        }

        #region Unimplemented members
        public bool FileExists(string path)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string path)
        {
            throw new NotImplementedException();
        }

        public string CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentDirectory()
        {
            throw new NotImplementedException();
        }

        public void Save(XDocument xml, string path)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class SaveRecordingsToMemory : RecordingXmlReader, ISaveRecordings
    {
        private readonly Stack<XDocument> _recordings;
        private readonly RecordingXmlWriter _xmlWriter;

        public SaveRecordingsToMemory()
        {
            _recordings = new Stack<XDocument>();
            _xmlWriter = new RecordingXmlWriter();
        }

        public void SaveMethodRecording(MethodRecording recording)
        {
            _recordings.Push(_xmlWriter.CreateXml(recording));
        }

        public override void LoadRecording(string path)
        {
            CurrentRecording = _recordings.Pop();
        }
    }
}