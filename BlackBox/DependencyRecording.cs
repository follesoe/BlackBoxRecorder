using System;
using System.Reflection;

namespace BlackBox
{
    public class DependencyRecording
    {
        public bool IsStatic { get; set; }
        public string TypeName { get; set; }
        public Type CalledOnType { get; set; }
        public MethodInfo Method { get; set; }
        public object ReturnValue { get; set; }

        public DependencyRecording()
        {
            
        }

        public DependencyRecording(object instance, MethodInfo method, object returnValue)
        {
            Method = method;
            ReturnValue = returnValue;
            CalledOnType = method.IsStatic ? method.DeclaringType : instance.GetType();
            IsStatic = method.IsStatic;
        }
    }
}
