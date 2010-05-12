using System;
using BlackBox.Recorder;

namespace BlackBox.Demo.App.MultipleCallsOnDependency
{
    [Dependency]
    public class RandomNumber
    {
        private Random _random = new Random(DateTime.Now.Millisecond);

        public int GetNumber()
        {
            return _random.Next(0, 100);
        }
    }
}
