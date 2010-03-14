using System.Xml.Linq;

using Moq;
using Xunit;

using BlackBox.Tests.Fakes;
using BlackBox.Recorder;


namespace BlackBox.Tests.Recorder
{
    public class SaveRecordingToDiskTest : BDD<SaveRecordingToDiskTest>
    {
        [Fact]
        public void TestBla()
        {

  
        }


        [Fact]
        public void Creates_directory_for_the_type_beeing_recoreded()
        {
            Given.there_is_no_directory_for_recording_on_type();
            When.we_try_to_save_a_recording();
            fileMock.VerifyAll();
        }

        [Fact]
        public void Creates_directory_for_the_method_beeing_recorded()
        {
            Given.there_is_no_directory_for_recording_of_method();
            When.we_try_to_save_a_recording();
            fileMock.VerifyAll();
        }

        [Fact]
        public void Saves_the_recording_as_XML_in_the_correct_folder()
        {
            Given.this_is_the_first_recording_of_method();
            When.we_try_to_save_a_recording();
            fileMock.VerifyAll();            
        }

        [Fact]
        public void Numbers_the_recording_name_if_method_has_been_saved_before()
        {
            Given.this_is_the_second_recording_of_a_method();
            When.we_try_to_save_a_recording();
            fileMock.VerifyAll();                    
        }

        private void this_is_the_second_recording_of_a_method()
        {
            there_is_no_directory_for_recording_of_method();

            fileMock.Setup(file => file.FileExists(RecordingPath)).Returns(true);
            fileMock.Setup(file => file.FileExists(RecordingPath2)).Returns(false);
            fileMock.Setup(file => file.Save(It.IsAny<XDocument>(), RecordingPath2)).Verifiable();
        }

        private void this_is_the_first_recording_of_method()
        {
            there_is_no_directory_for_recording_of_method();

            fileMock.Setup(file => file.FileExists(RecordingPath)).Returns(false);
            fileMock.Setup(file => file.Save(It.IsAny<XDocument>(), RecordingPath)).Verifiable();
        }

        private void there_is_no_directory_for_recording_on_type()
        {
            fileMock.Setup(file => file.DirectoryExists(TypeFolder)).Returns(false);
            fileMock.Setup(file => file.CreateDirectory(TypeFolder)).Returns("");
        }

        private void there_is_no_directory_for_recording_of_method()
        {
            fileMock.Setup(file => file.DirectoryExists(TypeFolder)).Returns(true);
            fileMock.Setup(file => file.DirectoryExists(MethodFolder)).Returns(false);
            fileMock.Setup(file => file.CreateDirectory(MethodFolder)).Returns("");
        }

        private const string TypeFolder = @"Recordings\BlackBox.Tests.Fakes.SimpleMath";
        private const string MethodFolder = @"Recordings\BlackBox.Tests.Fakes.SimpleMath\Int32 Add(Int32, Int32)";
        private const string RecordingPath = MethodFolder + @"\Int32_Add_Int32_Int32.xml";
        private const string RecordingPath2 = MethodFolder + @"\Int32_Add_Int32_Int32_2.xml";
   
        private void we_try_to_save_a_recording()
        {
            saveRecording.SaveMethodRecording(recorder.MethodRecordings[0]);
        }
        
        public SaveRecordingToDiskTest()
        {      
            simpleMath = new SimpleMath();
            simpleMath.Add(10, 10);
            
            recorder = (DefaultRecorder) RecordingServices.Recorder;

            fileMock = new Mock<IFile>();
            saveRecording = new SaveRecordingToDisk(fileMock.Object);
            RecordingServices.RecordingSaver = saveRecording;
        }

        private readonly DefaultRecorder recorder;        
        private readonly SaveRecordingToDisk saveRecording;        
        private readonly SimpleMath simpleMath;
        private readonly Mock<IFile> fileMock;
    }
}
