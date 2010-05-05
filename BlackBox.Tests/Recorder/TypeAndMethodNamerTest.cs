using System.Collections.Generic;
using System.Reflection;

using BlackBox.Recorder;
using BlackBox.Tests.Fakes;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.Recorder
{
    public class TypeAndMethodNamerTest
    {
        [Fact]
        public void Default_name_of_recording_should_start_with_type_and_method_name()
        {
            recorder.ClearRecordings();
            simpleMath.Add(5, 5);
            recorder.MethodRecordings[0].RecordingName.ShouldEqual("Add_a_b");
        }

        [Fact]
        public void Can_generate_method_name_with_parameters_from_method()
        {
            MethodInfo method = typeof (SimpleAddressBook).GetMethod("GetAllContacts", new[] {typeof (List<string>)});

            string name = method.GetMethodNameWithParameters();

            name.ShouldEqual("GetAllContacts(filters)");
        }

        private readonly SimpleMath simpleMath;
        private readonly DefaultRecorder recorder;

        public TypeAndMethodNamerTest()
        {
            RecordingServices.RecordingSaver = new DoNotSaveRecordings();
            recorder = (DefaultRecorder)RecordingServices.Recorder;
            RecordingServices.RecordingNamer = new TypeAndMethodNamer();

            simpleMath = new SimpleMath();
        }
    }
}