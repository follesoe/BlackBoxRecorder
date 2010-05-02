using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;

namespace BlackBox
{
    public class RecordingXmlReader
    {
        private XDocument _xml;

        public void LoadRecording(string path)
        {
            using(var sr = new StreamReader(path))
            {
                _xml = XDocument.Load(sr);
            }
        }

        public void LoadRecording(XDocument xml)
        {
            _xml = xml;
        }

        public string GetRecordingName()
        {
            return _xml.XPathSelectElement("/Recording/Name").Value;
        }

        public string GetTypeRecordingWasMadeOn()
        {
            return _xml.XPathSelectElement("/Recording/Type").Value;
        }

        public string GetMethodName()
        {
            return _xml.XPathSelectElement("/Recording/Method").Value;
        }

        public List<ParameterRecording> GetInputParametersMetadata()
        {
            return GetInputParameters(false);
        }

        public List<ParameterRecording> GetInputParameters()
        {
            return GetInputParameters(true);
        }

        public List<ParameterRecording> GetInputParameters(bool deserializeParameter)
        {
            return GetParameters("//InputParameters/Parameter", deserializeParameter);
        }

        public List<ParameterRecording> GetOutputParameters()
        {
            return GetOutputParameters(true);
        }

        public List<ParameterRecording> GetOutputParameters(bool deserializeParameter)
        {
            return GetParameters("//OutputParameters/Parameter", deserializeParameter);
        }

        private List<ParameterRecording> GetParameters(string xpath, bool deserializeParameter)
        {
            var parameters = new List<ParameterRecording>();
            foreach(var parameterNode in _xml.XPathSelectElements(xpath))
            {
                var parameter = new ParameterRecording();
                parameter.Name = parameterNode.Element("Name").Value;
                parameter.TypeName = parameterNode.Element("Type").Value;
                
                if (deserializeParameter)
                {
                    parameter.Type = Type.GetType(parameterNode.Element("FullyQualifiedType").Value);
                    parameter.Value = parameterNode.Element("Value").Value.Deserialize(parameter.Type);
                }

                parameters.Add(parameter);
            }
            return parameters;
        }

        public string GetTypeOfReturnValue()
        {
            return _xml.XPathSelectElement("/Recording/Return/Type").Value;
        }

        public object GetReturnValue()
        {
            string fullyQualifiedType = _xml.XPathSelectElement("/Recording/Return/FullyQualifiedType").Value;
            var type = Type.GetType(fullyQualifiedType);
            return _xml.XPathSelectElement("/Recording/Return/Value").Value.Deserialize(type);
        }
    }
}