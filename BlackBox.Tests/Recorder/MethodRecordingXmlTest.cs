using System.Xml.Linq;
using System.Collections.Generic;

using BlackBox.Recorder;
using BlackBox.Tests.Fakes;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.Recorder
{
    public class MethodRecordingXmlTest : BDD<MethodRecordingXmlTest>
    {
        [Fact]
        public void Can_format_recording_of_complex_types_as_XML()
        {
            Given.we_add_two_contacts();
            When.we_get_all_except_the_first_one();
            Then.we_should_be_able_to_format_the_recording_as_XML();
        }

        [Fact]
        public void Can_read_name_of_the_recording()
        {
            Given.we_have_an_xml_recording();
            When.we_load_the_recording_into_the_reader();
           
            string expectedIfUITestRunner = "Can_read_name_of_the_recording";
            string expectedIfConsoleTestRunner = "System.Collections.Generic.List`1[BlackBox.Tests.Fakes.Contact]_AllExcept_BlackBox.Tests.Fakes.Contact";
            string recordingName = reader.GetRecordingName();

            if(recordingName != expectedIfUITestRunner && recordingName != expectedIfConsoleTestRunner)
            {
                Assert.True(false, string.Format("Actual value ({0}) does not match any of the expected values ({1} or {2})", recordingName, expectedIfConsoleTestRunner, expectedIfUITestRunner));                
            }
        }

        [Fact]
        public void Can_read_name_of_the_type_that_was_recored()
        {
            Given.we_have_an_xml_recording();
            When.we_load_the_recording_into_the_reader();
            reader.GetTypeRecordingWasMadeOn().ShouldEqual("BlackBox.Tests.Fakes.SimpleAddressBook");
        }

        [Fact]
        public void Can_read_the_name_of_the_recorded_method()
        {
            Given.we_have_an_xml_recording();
            When.we_load_the_recording_into_the_reader();
            reader.GetMethodName().ShouldEqual("AllExcept");            
        }

        [Fact]
        public void Can_read_the_input_parameters_for_the_recored_method()
        {
            Given.we_have_an_xml_recording();
            When.we_load_the_recording_into_the_reader();
            reader.GetInputParameters().ShouldNotBeEmpty();
        }

        [Fact]
        public void Can_read_the_output_parameters_for_the_recorded_method()
        {
            Given.we_have_an_xml_recording();
            When.we_load_the_recording_into_the_reader();
            reader.GetOutputParameters().ShouldNotBeEmpty();
        }

        [Fact]
        public void Can_read_the_input_parameter_details_for_the_recored_method()
        {
            Given.we_have_an_xml_recording();
            When.we_load_the_recording_into_the_reader();
            reader.GetInputParameters()[0].Name.ShouldEqual("contact");
            reader.GetInputParameters()[0].Type.ShouldEqual(typeof(Contact));
            reader.GetInputParameters()[0].Value.ShouldNotBeNull();
        }

        [Fact]
        public void Can_read_the_output_parameter_details_for_the_recored_method()
        {
            Given.we_have_an_xml_recording();
            When.we_load_the_recording_into_the_reader();
            reader.GetOutputParameters()[0].Name.ShouldEqual("contact");
            reader.GetOutputParameters()[0].Type.ShouldEqual(typeof(Contact));
            reader.GetOutputParameters()[0].Value.ShouldNotBeNull();
        }

        [Fact]
        public void Can_read_the_name_of_the_return_type()
        {
            Given.we_have_an_xml_recording();
            When.we_load_the_recording_into_the_reader();
            reader.GetTypeOfReturnValue().ShouldEqual("System.Collections.Generic.List<BlackBox.Tests.Fakes.Contact>");
        }

        [Fact]
        public void Can_read_the_return_value()
        {
            Given.we_have_an_xml_recording();
            When.we_load_the_recording_into_the_reader();
            object returnValue = reader.GetReturnValue();
            returnValue.ShouldNotBeNull();
            returnValue.ShouldBeType(typeof(List<Contact>));
        }

        private Contact contact1, contact2;
        private readonly SimpleAddressBook addressBook;        
        private readonly RecordingXmlReader reader;
        private readonly RecordingXmlWriter writer;
        private readonly DefaultRecorder recorder;
        private XDocument xml;

        public MethodRecordingXmlTest()
        {
            RecordingServices.RecordingSaver = new DoNotSaveRecordings();

            writer = new RecordingXmlWriter();
            reader = new RecordingXmlReader();
            recorder = (DefaultRecorder)RecordingServices.Recorder;
            addressBook = new SimpleAddressBook();
        }

        private void we_load_the_recording_into_the_reader()
        {
            reader.LoadRecording(xml);
        }

        private void we_have_an_xml_recording()
        {
            recorder.ClearRecordings();

            we_add_two_contacts();
            we_get_all_except_the_first_one();
            we_should_be_able_to_format_the_recording_as_XML();
        }
        
        private void we_should_be_able_to_format_the_recording_as_XML()
        {
            recorder.MethodRecordings.ShouldNotBeEmpty();
            xml = writer.CreateXml(recorder.MethodRecordings[0]);
            xml.ShouldNotBeNull();
        }

        private void we_get_all_except_the_first_one()
        {
            var oneContact = addressBook.AllExcept(contact1);
            oneContact.ShouldContain(contact2); 
        }

        private void we_add_two_contacts()
        {
            contact1 = new Contact("Jonas Follesø", "jonas@follesoe.no");
            contact2 = new Contact("Hege Røkenes", "hege@rokenes.com");

            addressBook.AddContact(contact1);
            addressBook.AddContact(contact2);   
        }
    }
}