using BlackBox.Recorder;

using Xunit;
using Xunit.Extensions;

using BlackBox.Tests.Fakes;
using Moq;

namespace BlackBox.Tests.Recorder
{
    public class RecordAttributeTest
    {    
        [Fact]
        public void Records_each_call_to_the_method()
        {
            recorder.MethodRecordings.Count.ShouldEqual(3);
        }

        [Fact]
        public void Records_each_call_to_static_methods()
        {
            recorder.MethodRecordings[2].RecordingName.ShouldEqual("Int32_AddStatic_Int32_Int32");
        }

        [Fact]
        public void Recrods_the_instance_the_method_is_executed_on()
        {
            recorder.MethodRecordings[0].CalledOnType.ShouldEqual(typeof(SimpleMath));
        }

        [Fact]
        public void Creates_a_full_name_of_the_recorded_method()
        {
            recorder.MethodRecordings[0].MethodName.ShouldEqual("Int32 Add(Int32, Int32)");
        }

        [Fact]
        public void Records_the_method_input_parameters_and_return_value()
        {
            var inputParameter1 = (int)recorder.MethodRecordings[0].InputParameters[0].Value;
            var inputParameter2 = (int)recorder.MethodRecordings[0].InputParameters[1].Value;
            var returnValue = (int)recorder.MethodRecordings[0].ReturnValue;

            inputParameter1.ShouldEqual(5);
            inputParameter2.ShouldEqual(5);
            returnValue.ShouldEqual(10);
        }

        [Fact]
        public void Record_the_method_output_parameters()
        {
            var outputParameter1 = (int) recorder.MethodRecordings[0].OutputParameters[0].Value;
            var outputParameter2 = (int)recorder.MethodRecordings[0].OutputParameters[1].Value;
            
            outputParameter1.ShouldEqual(5);
            outputParameter2.ShouldEqual(5);
        }

        [Fact]
        public void Saves_the_recording()
        {
            saverMock.Verify();
        }

        public RecordAttributeTest()
        {
            saverMock = new Mock<ISaveRecordings>();
            saverMock.Setup(saver => saver.SaveMethodRecording(It.IsAny<MethodRecording>())).Verifiable();
            
            RecordingServices.RecordingSaver = saverMock.Object;
            RecordingServices.RecordingNamer = new TypeAndMethodNamer();
            recorder = (DefaultRecorder)RecordingServices.Recorder;
            recorder.ClearRecordings();

            math = new SimpleMath();
            math.Add(5, 5);
            math.Add(10, 10);
            SimpleMath.AddStatic(15, 15);
        }   

        private readonly SimpleMath math;
        private readonly DefaultRecorder recorder;
        private readonly Mock<ISaveRecordings> saverMock;
    }
}