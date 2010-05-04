using System;
using System.Linq;
using System.Reflection;
using PostSharp.Laos;

namespace BlackBox.Recorder
{
    [Serializable]
    public class DependencyAttribute : OnMethodInvocationAspect
    {
        public override void OnInvocation(MethodInvocationEventArgs eventArgs)
        {
            if (RecordingServices.Configuration.IsRecording())
            {
                eventArgs.ReturnValue = eventArgs.Method.Invoke(eventArgs.Instance, eventArgs.GetArgumentArray());
                if (RecordingStack.Count > 0)
                {
                    Guid callGuid = RecordingStack.Peek();
                    RecordingServices.Recorder.RecordDependency(callGuid, eventArgs.Instance, eventArgs.Method, eventArgs.ReturnValue);
                }
            }
            else
            {
                var publicMethod = GetInterceptedMethod(eventArgs.Method);
                if (RecordingServices.DependencyPlayback.HasReturnValue(publicMethod))
                {
                    eventArgs.ReturnValue = RecordingServices.DependencyPlayback.GetReturnValue(publicMethod);
                }
                else
                {
                    eventArgs.ReturnValue = eventArgs.Method.Invoke(eventArgs.Instance, eventArgs.GetArgumentArray());
                }
            }
        }

        private static MethodInfo GetInterceptedMethod(MethodInfo interceptionMethod)
        {
            var parameterTypes = from parameter in interceptionMethod.GetParameters()
                                 select parameter.ParameterType;

            return interceptionMethod.DeclaringType.GetMethod(interceptionMethod.GetMethodNameWithoutTilde(), parameterTypes.ToArray());
        }
    }
}
