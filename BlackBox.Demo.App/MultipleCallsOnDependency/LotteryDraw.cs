using BlackBox.Recorder;

namespace BlackBox.Demo.App.MultipleCallsOnDependency
{
    public class LotteryDraw
    {
        [Recording]
        public int[] GenerateLotteryNumbers()
        {
            var random = new RandomNumber();

            return new[]
                       {
                           random.GetNumber(),
                           random.GetNumber(),
                           random.GetNumber(),
                           random.GetNumber(),
                           random.GetNumber(),
                           random.GetNumber()
                       };
        }
    }
}
