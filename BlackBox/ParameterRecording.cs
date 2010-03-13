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

        public ParameterRecording(string name, object value)
        {
            Name = name;
            Value = value;
            Type = value.GetType();
        }
    }
}