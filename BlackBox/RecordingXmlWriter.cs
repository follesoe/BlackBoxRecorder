using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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
                             new XElement("Type", new XCData(recording.CalledOnType.GetCodeDefinition())),
                             new XElement("FullyQualifiedType", new XCData(recording.CalledOnType.AssemblyQualifiedName)),
                             new XElement("InputParameters", CreateParameterNodes(recording.InputParameters)),
                             new XElement("OutputParameters", CreateParameterNodes(recording.OutputParameters)),
                             new XElement("Return",
                                          new XElement("Type", new XCData(recording.ReturnValue.GetType().GetCodeDefinition())),
                                          new XElement("FullyQualifiedType", new XCData(recording.ReturnValue.GetType().AssemblyQualifiedName)),
                                          new XElement("Value", new XCData(recording.ReturnValue.ToXml().ToString()))
                                 )));
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
    }
}