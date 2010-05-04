using System.Reflection;


namespace BlackBox.Recorder
{
    public class TypeAndMethodNamer : INameRecordings
    {
        public string GetNameForRecording(MethodBase method)
        {
            return method.GetMethodNameWithParameters();
        }
    }
}