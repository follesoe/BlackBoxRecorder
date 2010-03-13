using BlackBox.Recorder;

namespace BlackBox.Tests.Fakes
{
    public class SimpleMathFacade
    {
        private readonly SimpleMath _math;

        public SimpleMathFacade()
        {
            _math = new SimpleMath();
        }

        [Recording]
        public int Add(int a, int b)
        {
            return _math.Add(a, b);
        }
    }
}
