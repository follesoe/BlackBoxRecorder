using BlackBox.Recorder;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.Recorder
{
    public class RecordingOnTypeLevelTest
    {
        [Fact]
        public void Should_not_record_constructor_on_type_marked_for_recording()
        {
            recorder.ClearRecordings();

            new TypeWithRecording();
            recorder.MethodRecordings.ShouldBeEmpty();
        }

        [Fact]
        public void Should_record_methods_on_type_marked_for_recording()
        {
            recorder.ClearRecordings();

            var typeWithRecording = new TypeWithRecording();
            int sum = typeWithRecording.Add(1, 2);
            recorder.MethodRecordings.ShouldNotBeEmpty();
        }

        private readonly DefaultRecorder recorder;

        public RecordingOnTypeLevelTest()
        {
            RecordingServices.RecordingSaver = new DoNotSaveRecordings();
            recorder = (DefaultRecorder)RecordingServices.Recorder;    
        }
    }

    [Recording]
    public class TypeWithRecording
    {
        public TypeWithRecording()
        {
            
        }

        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
