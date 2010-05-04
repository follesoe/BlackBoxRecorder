using BlackBox.Recorder;
using BlackBox.Tests.Fakes;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.Recorder
{
    public class DependencyAttributeTest
    {
        [Fact]
        public void Records_dependencies_if_called_inside_method_recording()
        {                       
            recorder.MethodRecordings[0].DependencyRecordings.ShouldNotBeEmpty();
        }



        public DependencyAttributeTest()
        {
            RecordingServices.RecordingSaver = new DoNotSaveRecordings();
            recorder = (DefaultRecorder)RecordingServices.Recorder;
            recorder.ClearRecordings();

            addressBook = new SimpleAddressBook();
            addressBook.GetAllContacts();         
        }

        private SimpleAddressBook addressBook;
        private DefaultRecorder recorder;
    }
}
