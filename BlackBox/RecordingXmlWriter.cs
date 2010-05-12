using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace BlackBox
{
    public class RecordingXmlWriter
    {
        public XDocument CreateXml(MethodRecording recording)
        {
            return new XDocument(
                new XElement("Recording",
                             new XElement("Name", recording.RecordingName),
                             new XElement("Method", recording.Method.Name),
                             new XElement("IsStatic", recording.Method.IsStatic),
                             new XElement("Type", new XCData(recording.CalledOnType.GetCodeDefinition())),
                             new XElement("FullyQualifiedType", new XCData(recording.CalledOnType.AssemblyQualifiedName)),
                             new XElement("InputParameters", CreateParameterNodes(recording.InputParameters)),
                             new XElement("OutputParameters", CreateParameterNodes(recording.OutputParameters)),
                             new XElement("Return", CreateReturnNode(recording),
                             new XElement("Dependencies", CreateDependencyNodes(recording.DependencyRecordings)))));
        }

        private static IEnumerable<XElement> CreateReturnNode(MethodRecording recording)
        {
            if(recording.ReturnValue == null)
            {
                yield return new XElement("Type", "void");
                yield return new XElement("FullyQualifiedType", "void");
                yield return new XElement("Value", "void");
                yield break;
            }

            yield return new XElement("Type", new XCData(recording.ReturnValue.GetType().GetCodeDefinition()));
            yield return new XElement("FullyQualifiedType", new XCData(recording.ReturnValue.GetType().AssemblyQualifiedName));
            yield return new XElement("Value", new XCData(recording.ReturnValue.ToXml().ToString()));
        }

        private static IEnumerable<XElement> CreateParameterNodes(IEnumerable<ParameterRecording> parameterRecordings)
        {
            return from parameter in parameterRecordings
                   select new XElement("Parameter",
                                       new XElement("Name", parameter.Name),
                                       new XElement("Type", new XCData(parameter.Value.GetType().GetCodeDefinition())),
                                       new XElement("FullyQualifiedType", new XCData(parameter.Value.GetType().AssemblyQualifiedName)),
                                       new XElement("Value", new XCData(parameter.Value.ToXml().ToString())));
        }

        private static IEnumerable<XElement> CreateDependencyNodes(IEnumerable<DependencyRecording> dependencyRecordings)
        {
            return from dependency in dependencyRecordings
                   select new XElement("Dependency",
                                       new XElement("Type", new XCData(dependency.CalledOnType.GetCodeDefinition())),
                                       new XElement("FullyQualifiedType", new XCData(dependency.CalledOnType.AssemblyQualifiedName)),
                                       new XElement("Method",
                                           new XElement("Name", dependency.Method.GetMethodNameWithoutTilde()),
                                           new XElement("IsStatic", dependency.Method.IsStatic),
                                           new XElement("Parameters",
                                               from parameter in dependency.Method.GetParameters()
                                               select new XElement("FullyQualifiedType", parameter.ParameterType.AssemblyQualifiedName)),
                                           new XElement("ReturnValues",
                                               from returnValue in dependency.ReturnValues
                                               select new XElement("ReturnValue",
                                                   new XElement("FullyQualifiedType", returnValue.GetType().AssemblyQualifiedName),
                                                   new XElement("Value", new XCData(returnValue.ToXml().ToString()))))));
        }
    }
}