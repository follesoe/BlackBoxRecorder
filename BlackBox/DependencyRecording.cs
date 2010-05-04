using System;
using System.Collections.Generic;
using System.Reflection;

namespace BlackBox
{
    public class DependencyRecording
    {
        public bool IsStatic { get; set; }
        public string TypeName { get; set; }
        public Type CalledOnType { get; set; }
        public MethodInfo Method { get; set; }
        public List<object> ReturnValues { get; set; }

        public DependencyRecording()
        {
            ReturnValues = new List<object>();
        }

        public DependencyRecording(object instance, MethodInfo method)
        {
            Method = method;
            ReturnValues = new List<object>();
            
            CalledOnType = method.IsStatic ? method.DeclaringType : instance.GetType();
            IsStatic = method.IsStatic;
        }

        public void AddReturnValue(object returnValue)
        {
            ReturnValues.Add(returnValue);
        }
    }
}
