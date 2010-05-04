using System.Reflection;
using System.Collections.Generic;

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
            addressBook.GetAllContacts();            
            recorder.MethodRecordings[0].DependencyRecordings.ShouldNotBeEmpty();
        }

        [Fact]
        public void Returns_recorded_value_if_on_playback_mode()
        {           
            MethodInfo method = typeof (SimpleAddressBookDb).GetMethod("GetContacts");
            var contactsToReturn = new List<Contact> {new Contact("BlackBox", "blackbox@gmail.com")};
            RecordingServices.DependencyPlayback.RegisterExpectedReturnValue(method, contactsToReturn);

            RecordingServices.Configuration.RecordingMode = RecordingMode.Playback;
            var contacts = addressBook.GetAllContacts();
            RecordingServices.Configuration.RecordingMode = RecordingMode.Recording;
            
            contacts.ShouldContain(contactsToReturn[0]);
        }

        public DependencyAttributeTest()
        {
            RecordingServices.RecordingSaver = new DoNotSaveRecordings();
            recorder = (DefaultRecorder)RecordingServices.Recorder;
            recorder.ClearRecordings();

            addressBook = new SimpleAddressBook();
                 
        }

        private SimpleAddressBook addressBook;
        private DefaultRecorder recorder;
    }
}
