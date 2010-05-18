using System;
using BlackBox.Recorder;

namespace BlackBox.Demo.App.ExternalApiDependencyRecording
{
    public class LotteryDraw
    {
        [Recording]
        public int[] GenerateNumbers()
        {
            var random = new Random(DateTime.Now.Millisecond);

            return new[]
                       {
                           random.Next(1, 100),
                           random.Next(1, 100),
                           random.Next(1, 100),
                           random.Next(1, 100),
                           random.Next(1, 100),
                           random.Next(1, 100)
                       };
        }
    }
}
