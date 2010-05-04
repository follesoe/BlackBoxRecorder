using System.Xml.Linq;

using BlackBox.Recorder;
using BlackBox.Testing;
using BlackBox.Tests.Fakes;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.Testing
{
    public class CharacterizationTestBaseClassTest : BDD<CharacterizationTestBaseClassTest>
    {
        [Fact]
        public void Can_load_a_test_recording_as_XDocument()
        {
            Given.we_have_a_test_recording_as_xml();
            testClass.LoadRecording(recording);
        }        

        [Fact]
        public void Can_get_the_value_of_a_parameter()
        {
            Given.we_have_a_test_recording_as_xml();
            testClass.LoadRecording(recording);
            testClass.GetInputParameterValue("filter").ShouldNotBeNull();
        }

        [Fact]
        public void Can_get_the_return_value_of_a_recording()
        {
            Given.we_have_a_test_recording_as_xml();
            testClass.LoadRecording(recording);
            testClass.GetReturnValue().ShouldNotBeNull();
        }

        [Fact]
        public void Can_change_from_recording_to_playback_mode()
        {
            Given.we_have_a_test_recording_as_xml();
            testClass.LoadRecording(recording);
            testClass.Initialize();

            RecordingServices.Configuration.RecordingMode.ShouldEqual(RecordingMode.Playback);
            RecordingServices.Configuration.RecordingMode = RecordingMode.Recording;
        }

        [Fact]
        public void Loads_return_values_for_external_dependencies_on_initialize()
        {
            Given.we_have_a_test_recording_as_xml();
            testClass.LoadRecording(recording);
            var externalMethod = typeof (SimpleAddressBookDb).GetMethod("GetContacts");
            
            RecordingServices.DependencyPlayback.HasReturnValue(externalMethod).ShouldBeTrue();
        }
        
        private readonly CharacterizationTest testClass;
        private readonly SimpleAddressBook addressBook;                
        private readonly RecordingXmlWriter writer;
        private readonly DefaultRecorder recorder;
        private XDocument recording;

        public CharacterizationTestBaseClassTest()
        {
            RecordingServices.RecordingSaver = new DoNotSaveRecordings();
            recorder = (DefaultRecorder)RecordingServices.Recorder;

            testClass = new CharacterizationTest();
            writer = new RecordingXmlWriter();            
            addressBook = new SimpleAddressBook();            
        }        

        private void we_have_a_test_recording_as_xml()
        {
            recorder.ClearRecordings();
            addressBook.GetAllContacts("some filter");

            recording = writer.CreateXml(recorder.MethodRecordings[0]);
        }
    }
}
