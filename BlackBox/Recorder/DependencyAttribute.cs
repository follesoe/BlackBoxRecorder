using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using PostSharp.Aspects;

namespace BlackBox.Recorder
{
    [Serializable]
    public class DependencyAttribute : MethodInterceptionAspect
    {
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
                    Debugger.Break();
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
            return (method.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0 && 
                !method.Name.StartsWith("get_") && 
                !method.Name.StartsWith("set_"));
        }
    }
}
