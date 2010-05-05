using System.Reflection;

namespace BlackBox.Recorder
{
    public class TypeAndMethodNamer : INameRecordings
    {
        public string GetNameForRecording(MethodBase method)
        {
            string name = method.Name;

            foreach(var parameter in method.GetParameters())
            {
                name += "_" + parameter.Name;
            }

            return name;
        }
    }
}