using BlackBox.CodeGeneration;
using BlackBox.Recorder;

namespace BlackBox
{
    public static class RecordingServices
    {        
        public static IRecordMethodCalls Recorder { get; set; }
        public static INameRecordings RecordingNamer { get; set; }
        public static ISaveRecordings RecordingSaver { get; set; }
        public static Configuration Configuration { get; private set; }
        public static DependencyPlayback DependencyPlayback { get; private set; }

        static RecordingServices()
        {
            Recorder = new DefaultRecorder();
            RecordingNamer = new CallStackRecordingNamer();
            RecordingSaver = new SaveRecordingToDisk(new FileAdapter(), new TestGenerator());
            
            DependencyPlayback = new DependencyPlayback();
            Configuration = new Configuration();
        }
    }
}