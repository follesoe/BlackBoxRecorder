using System;

namespace BlackBox
{
    public class ParameterRecording
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public object Value { get; set; }        
        public Type Type { get; set; }

        public ParameterRecording()
        {
            
        }

        public ParameterRecording(string name, object value, Type type)
        {
            Name = name;
            Value = value;
            Type = type;
            TypeName = Type.GetCodeDefinition();
            Value = value;
        }
    }
}