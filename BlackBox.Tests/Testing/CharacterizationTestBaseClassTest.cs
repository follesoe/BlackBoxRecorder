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
            testClass.Initialize();
            RecordingServices.Configuration.RecordingMode.ShouldEqual(RecordingMode.Playback);
            RecordingServices.Configuration.RecordingMode = RecordingMode.Recording;
        }
        
        private readonly CharacterizationTest testClass;
        private readonly SimpleAddressBook addressBook;                
        private readonly RecordingXmlWriter writer;
        private readonly DefaultRecorder recorder;
        private XDocument recording;

        public CharacterizationTestBaseClassTest()
        {
            testClass = new CharacterizationTest();
            writer = new RecordingXmlWriter();
            recorder = (DefaultRecorder)RecordingServices.Recorder;
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
