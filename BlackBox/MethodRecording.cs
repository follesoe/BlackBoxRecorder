using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace BlackBox
{
    public class MethodRecording
    {
        public List<DependencyRecording> DependencyRecordings { get; private set; }
        public List<ParameterRecording> InputParameters { get; private set; }
        public List<ParameterRecording> OutputParameters { get; private set; }
        public string RecordingName { get; private set; }
        public MethodBase Method { get; private set; }
        public Type CalledOnType { get; private set; }        
        public object ReturnValue { get; private set; }

        public string MethodName 
        {
            get { return Method.GetMethodNameWithParameters(); }
        }        

        public MethodRecording(MethodBase method, object instance, object[] parameterValues)
        {
            RecordingName = RecordingServices.RecordingNamer.GetNameForRecording(method);
            InputParameters = new List<ParameterRecording>();
            OutputParameters = new List<ParameterRecording>();
            DependencyRecordings = new List<DependencyRecording>();

            AddMethod(method, instance, parameterValues);
        }

        private void AddMethod(MethodBase method, object instance, object[] parameterValues)
        {
            Method = method;
            CalledOnType = method.IsStatic ? method.DeclaringType : instance.GetType();            

            AddParameters(parameterValues, InputParameters);
        }

        public void AddReturnValues(object[] parameterValues, object returnValue)
        {
            ReturnValue = returnValue.Copy();
            AddParameters(parameterValues, OutputParameters);
        }

        public void AddDependency(object dependencyInstance, MethodInfo method, object returnValue)
        {
            var recording = (from r in DependencyRecordings
                             where r.Method == method
                             select r).SingleOrDefault();

            if (recording == null)
            {
                recording = new DependencyRecording(dependencyInstance, method);
                DependencyRecordings.Add(recording);
            }

            recording.AddReturnValue(returnValue);
        }

        private void AddParameters(object[] sourceParameters, List<ParameterRecording> targetParameters)
        {
            ParameterInfo[] parameters = Method.GetParameters();
            for (int i = 0; i < parameters.Length; ++i)
            {
                string name = parameters[i].Name;
                object value = sourceParameters[i].Copy();
                targetParameters.Add(new ParameterRecording(name, value));
            }
        }
    }
}