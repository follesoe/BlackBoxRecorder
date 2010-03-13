using BlackBox.Recorder;

namespace BlackBox.Tests.Fakes
{
    public class SimpleMath
    {
        [Recording]
        public int Add(int a, int b)
        {
            return a + b;
        }

        [Recording]
        public static int AddStatic(int a, int b)
        {
            return a + b;
        }
    }
}