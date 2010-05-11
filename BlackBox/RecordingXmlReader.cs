using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;

namespace BlackBox
{
    public class RecordingXmlReader
    {
        protected XDocument CurrentRecording { get; set; }

        public virtual void LoadRecording(string path)
        {
            using(var sr = new StreamReader(path))
            {
                CurrentRecording = XDocument.Load(sr);
            }
        }

        public void LoadRecording(XDocument xml)
        {
            CurrentRecording = xml;
        }

        public string GetRecordingName()
        {
            return CurrentRecording.XPathSelectElement("/Recording/Name").Value;
        }

        public string GetTypeRecordingWasMadeOn()
        {
            return CurrentRecording.XPathSelectElement("/Recording/Type").Value;
        }

        public string GetMethodName()
        {
            return CurrentRecording.XPathSelectElement("/Recording/Method").Value;
        }

        public bool GetMethodIsStatic()
        {
            return Convert.ToBoolean(CurrentRecording.XPathSelectElement("/Recording/IsStatic").Value);
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

        public List<ParameterRecording> GetOutputParametersMetadata()
        {
            return GetOutputParameters(false);
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
            foreach(var parameterNode in CurrentRecording.XPathSelectElements(xpath))
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

        public List<DependencyRecording> GetDependencies()
        {
            var dependencies = new List<DependencyRecording>();
            foreach(var dependencyNode in CurrentRecording.XPathSelectElements("//Dependency"))
            {
                var dependency = new DependencyRecording();
                dependency.TypeName = dependencyNode.Element("Type").Value;
                dependency.CalledOnType = Type.GetType(dependencyNode.Element("FullyQualifiedType").Value);

                dependency.IsStatic = Convert.ToBoolean(dependencyNode.XPathSelectElement("Method/IsStatic").Value);                                
                string methodName = dependencyNode.XPathSelectElement("Method/Name").Value;
                var methodParameters = new List<Type>();

                foreach (var parameterNode in dependencyNode.XPathSelectElements("Method//Parameters"))
                {
                    if (!string.IsNullOrEmpty(parameterNode.Value))
                    {
                        methodParameters.Add(Type.GetType(parameterNode.Value));
                    }
                }

                foreach(var returnNode in dependencyNode.XPathSelectElements("Method//ReturnValue"))
                {
                    Type returnType = Type.GetType(returnNode.Element("FullyQualifiedType").Value);
                    object returnValue = returnNode.Element("Value").Value.Deserialize(returnType);
                    dependency.AddReturnValue(returnValue);
                }

                dependency.Method = dependency.CalledOnType.GetMethod(methodName, methodParameters.ToArray());                

                dependencies.Add(dependency);
            }
            return dependencies;
        }

        public string GetTypeOfReturnValue()
        {
            return CurrentRecording.XPathSelectElement("/Recording/Return/Type").Value;
        }

        public object GetReturnValue()
        {
            string fullyQualifiedType = CurrentRecording.XPathSelectElement("/Recording/Return/FullyQualifiedType").Value;
            var type = Type.GetType(fullyQualifiedType);
            return CurrentRecording.XPathSelectElement("/Recording/Return/Value").Value.Deserialize(type);
        }

        public bool IsMethodVoid()
        {
            string fullyQualifiedType = CurrentRecording.XPathSelectElement("/Recording/Return/FullyQualifiedType").Value;
            return fullyQualifiedType.Equals("void");
        }
    }
}