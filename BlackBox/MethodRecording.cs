using System;
using System.Collections.Generic;
using System.Reflection;
using OX.Copyable;

namespace BlackBox
{
    public class MethodRecording
    {
        public List<ParameterRecording> Parameters { get; private set; }
        public string RecordingName { get; private set; }
        public MethodBase Method { get; private set; }
        public Type CalledOnType { get; private set; }        
        public object ReturnValue { get; set; }

        public string MethodName 
        {
            get { return Method.ToString(); }
        }

        public MethodRecording(string recordingName, MethodBase method, object instance, object[] parameterValues)
        {
            RecordingName = recordingName;
            Parameters = new List<ParameterRecording>();
            AddMethod(method, instance, parameterValues);
        }

        private void AddMethod(MethodBase method, object instance, object[] parameterValues)
        {
            Method = method;
            CalledOnType = method.IsStatic ? method.DeclaringType : instance.GetType();            

            ParameterInfo[] parameters = method.GetParameters();
            for(int i = 0; i < parameters.Length; ++i)
            {
                string name = parameters[i].Name;
                object value = parameterValues[i].Copy();                
                Parameters.Add(new ParameterRecording(name, value));
            }
        }
    }
}