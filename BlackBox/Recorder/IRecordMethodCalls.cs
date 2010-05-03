using System;
using System.Reflection;

namespace BlackBox.Recorder
{
    public interface IRecordMethodCalls
    {        
        void RecordEntry(Guid callGuid, MethodBase methodBase, object instance, object[] inputParameters);
        void RecordExit(Guid callGuid, object[] outputParameters, object returnValue);
        void RecordDependency(Guid callGuid, object dependencyInstance, MethodInfo method, object returnValue);
    }
}