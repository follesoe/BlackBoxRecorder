using BlackBox.Recorder;
using BlackBox.Tests.Fakes;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.Recorder
{
    public class RecordingOfComplexTypesTest : BDD<RecordingOfComplexTypesTest>
    {
        [Fact]
        public void Records_complex_parameters_and_return_values()
        {           
            Given.we_add_two_contacts();
            When.we_get_all_except_the_first_one();
            Then.the_call_to_remove_all_should_be_recorded();
        }
        
        private void the_call_to_remove_all_should_be_recorded()
        {            
            _recorder.MethodRecordings.ShouldNotBeEmpty();       
        }

        private void we_get_all_except_the_first_one()
        {
            var oneContact = _addressBook.AllExcept(_contact1);
            oneContact.ShouldContain(_contact2); 
        }

        private void we_add_two_contacts()
        {
            _contact1 = new Contact("Jonas Follesø", "jonas@follesoe.no");
            _contact2 = new Contact("Hege Røkenes", "hege@rokenes.com");

            _addressBook.AddContact(_contact1);
            _addressBook.AddContact(_contact2);   
        }

        private readonly DefaultRecorder _recorder;
        private readonly SimpleAddressBook _addressBook;
        private Contact _contact1, _contact2;

        public RecordingOfComplexTypesTest()
        {
            RecordingServices.RecordingSaver = new DoNotSaveRecordings();
            _recorder = (DefaultRecorder)RecordingServices.Recorder;
            _addressBook = new SimpleAddressBook();    
        }
    }
}