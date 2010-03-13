using System.Reflection;

namespace BlackBox.Recorder
{
    public class TypeAndMethodNamer : INameRecordings
    {
        public string GetNameForRecording(MethodBase method)
        {
            string methodAsString = method.ToString();
            methodAsString = methodAsString.Replace("(", " ");
            methodAsString = methodAsString.Replace(")", "");
            methodAsString = methodAsString.Replace(",", "");
            methodAsString = methodAsString.Replace(" ", "_");            
            return methodAsString;
        }
    }
}