using BlackBox.Recorder;
using Xunit;
using Xunit.Extensions;

using BlackBox.Tests.Fakes;

namespace BlackBox.Tests.Recorder
{
    public class CallStackNamingOfRecordingTest
    {
        [Fact]
        public void Name_of_recording_should_match_test_name()
        {
            recorder.ClearRecordings();
            simpleMath.Add(5, 5);
            recorder.MethodRecordings[0].RecordingName.ShouldEqual("Name_of_recording_should_match_test_name");            
        }

        [Fact]
        public void Name_of_recording_should_match_test_name_for_non_direct_call()
        {
            recorder.ClearRecordings();
            simpleMathFacade.Add(5, 5);
            recorder.MethodRecordings[0].RecordingName.ShouldEqual("Name_of_recording_should_match_test_name_for_non_direct_call");
        }

        private readonly SimpleMath simpleMath;
        private readonly SimpleMathFacade simpleMathFacade;
        private readonly DefaultRecorder recorder;

        public CallStackNamingOfRecordingTest()
        {
            recorder = (DefaultRecorder)RecordingServices.Recorder;
            RecordingServices.RecordingNamer = new CallStackRecordingNamer();

            simpleMath = new SimpleMath();
            simpleMathFacade = new SimpleMathFacade();
        }
    }
}