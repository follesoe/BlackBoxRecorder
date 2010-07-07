using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace BlackBox.Recorder
{
    [Serializable]
    [MulticastAttributeUsageAttribute(MulticastTargets.Method)]
    public class DependencyAttribute : MethodInterceptionAspect
    {
        public DependencyAttribute()
        {
            AttributeTargetElements = MulticastTargets.Method;
        }

        public override void OnInvoke(MethodInterceptionArgs eventArgs)
        {            
            if (Configuration.IsRecording())
            {
                eventArgs.Proceed();
                if (RecordingStack.Count > 0)
                {
                    Guid callGuid = RecordingStack.Peek();
                    RecordingServices.Recorder.RecordDependency(callGuid, eventArgs.Instance, (MethodInfo)eventArgs.Method, eventArgs.ReturnValue);
                }
            }
            else
            {                
                if (RecordingServices.DependencyPlayback.HasReturnValue((MethodInfo)eventArgs.Method))
                {
                    eventArgs.ReturnValue = RecordingServices.DependencyPlayback.GetReturnValue((MethodInfo)eventArgs.Method);
                }
                else
                {
                    eventArgs.Proceed();
                }
            }
        }

        public override bool CompileTimeValidate(MethodBase method)
        {
            return 
                (method.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0 && 
                !method.Name.StartsWith("get_") && 
                !method.Name.StartsWith("set_") &&
                !method.IsConstructor);
        }
    }
}
