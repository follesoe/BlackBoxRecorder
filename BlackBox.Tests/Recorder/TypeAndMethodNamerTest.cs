using BlackBox.Recorder;
using BlackBox.Tests.Fakes;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.Recorder
{
    public class TypeAndMethodNamerTest
    {
        [Fact]
        public void Default_name_of_recording_should_start_with_type_and_method_name()
        {
            recorder.ClearRecordings();
            simpleMath.Add(5, 5);
            recorder.MethodRecordings[0].RecordingName.ShouldEqual("Int32_Add_Int32_Int32");
        }

        private readonly SimpleMath simpleMath;
        private readonly DefaultRecorder recorder;

        public TypeAndMethodNamerTest()
        {
            RecordingServices.RecordingSaver = new DoNotSaveRecordings();
            recorder = (DefaultRecorder)RecordingServices.Recorder;
            RecordingServices.RecordingNamer = new TypeAndMethodNamer();

            simpleMath = new SimpleMath();
        }
    }
}