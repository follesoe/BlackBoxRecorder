using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace BlackBox.Testing
{
    public class CharacterizationTest
    {
        private readonly RecordingXmlReader _reader;
        private readonly List<ParameterRecording> _parameters;
       
        public CharacterizationTest()
        {
            _reader = new RecordingXmlReader();
            _parameters = new List<ParameterRecording>();
        }

        public void LoadRecording(string path)
        {
            LoadRecording(XDocument.Load(path));
        }

        public void LoadRecording(XDocument recording)
        {
            _parameters.Clear();
            _reader.LoadRecording(recording);
            LoadDependenyReturnValues();
        }

        private void LoadDependenyReturnValues()
        {
            List<DependencyRecording> recordedDependencies = _reader.GetDependencies();

            foreach (var dependency in recordedDependencies)
            {
                foreach (var returnValue in dependency.ReturnValues)
                {
                    RecordingServices.DependencyPlayback.RegisterExpectedReturnValue(dependency.Method, returnValue);
                }
            }
        }

        public object GetInputParameterValue(string parameterName)
        {
            if (_parameters.Count == 0)
            {
                _parameters.AddRange(_reader.GetInputParameters());
            }
            return _parameters.Where(p => p.Name == parameterName).SingleOrDefault().Value;
        }

        public object GetReturnValue()
        {
            return _reader.GetReturnValue();   
        }

        public void CompareObjects(object expected, object actuall)
        {

        }

        public void Initialize()
        {
            RecordingServices.Configuration.RecordingMode = RecordingMode.Playback;
        }
    }
}