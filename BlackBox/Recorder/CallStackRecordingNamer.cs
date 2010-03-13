using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace BlackBox.Recorder
{
    public class CallStackRecordingNamer : INameRecordings
    {
        private readonly TypeAndMethodNamer _fallbackNamer;

        public CallStackRecordingNamer()
        {
            _fallbackNamer = new TypeAndMethodNamer();
        }
        
        public string GetNameForRecording(MethodBase method)
        {
            var stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();
            StackFrame testMethod = FindTestMethod(stackFrames);
            
            if(testMethod == null)
            {
                return _fallbackNamer.GetNameForRecording(method);
            }

            return testMethod.GetMethod().Name;
        }

        private StackFrame FindTestMethod(IEnumerable<StackFrame> stackFrames)
        {
            return (from frame in stackFrames 
                    let method = frame.GetMethod() 
                    where HasTestAttribute(method) 
                    select frame).FirstOrDefault();
        }

        private bool HasTestAttribute(MemberInfo method)
        {
            Attribute[] attributes = Attribute.GetCustomAttributes(method, true);

            foreach(var attribute in attributes)
            {
                string typeName = attribute.GetType().Name;
                if(IsTestAttributeName(typeName)) return true;
            }

            return false;
        }

        private bool IsTestAttributeName(string attributeName)
        {
            attributeName = attributeName.ToLower();
            return attributeName.Contains("test") || attributeName.Contains("fact");
        }
    }
}