using System;
using BlackBox.Recorder;
using PostSharp.Extensibility;

namespace BlackBox.Demo.App.MultipleCallsOnDependency
{
    [Dependency]
    public class RandomNumber
    {
        private readonly Random _random = new Random(DateTime.Now.Millisecond);

        public int GetNumber()
        {
            return _random.Next(0, 100);
        }
    }
}
