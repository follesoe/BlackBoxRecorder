using System.Reflection;

namespace BlackBox.Recorder
{
    public interface INameRecordings
    {
        string GetNameForRecording(MethodBase method);
    }
}