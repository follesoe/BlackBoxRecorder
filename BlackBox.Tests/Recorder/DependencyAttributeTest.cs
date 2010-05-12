using System;
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
        public void Returns_recorded_value_if_in_playback_mode()
        {           
            MethodInfo method = typeof (SimpleAddressBookDb).GetMethod("GetContacts");
            var contactsToReturn = new List<Contact> {new Contact("BlackBox", "blackbox@gmail.com")};
            RecordingServices.DependencyPlayback.RegisterExpectedReturnValue(method, contactsToReturn);

            Configuration.RecordingMode = RecordingMode.Playback;
            var contacts = addressBook.GetAllContacts();
            Configuration.RecordingMode = RecordingMode.Recording;

            contacts.ShouldContain(contactsToReturn[0]);
        }

        [Fact]
        public void Returns_recorded_value_if_in_playback_mode_on_static_methods()
        {
            MethodInfo method = typeof(SimpleAddressBookDb).GetMethod("GetContactsStatic");
            var contactsToReturn = new List<Contact> { new Contact("BlackBoxStatic", "blackbox@gmail.com") };
            RecordingServices.DependencyPlayback.RegisterExpectedReturnValue(method, contactsToReturn);

            Configuration.RecordingMode = RecordingMode.Playback;
            var contacts = addressBook.GetAllContactsViaStatic();
            Configuration.RecordingMode = RecordingMode.Recording;

            contacts.ShouldContain(contactsToReturn[0]);
        }

        public DependencyAttributeTest()
        {
            RecordingServices.RecordingSaver = new DoNotSaveRecordings();
            recorder = (DefaultRecorder)RecordingServices.Recorder;
            recorder.ClearRecordings();

            addressBook = new SimpleAddressBook();
                 
        }

        private readonly SimpleAddressBook addressBook;
        private readonly DefaultRecorder recorder;
    }
}
