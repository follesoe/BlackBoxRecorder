namespace BlackBox
{
    public class Configuration
    {
        public RecordingMode RecordingMode { get; set; }

        public bool IsRecording()
        {
            return RecordingMode == RecordingMode.Recording;
        }

        public bool IsPlayback()
        {
            return RecordingMode == RecordingMode.Playback;
        }

        public Configuration()
        {
            RecordingMode = RecordingMode.Recording;
        }
    }
}
