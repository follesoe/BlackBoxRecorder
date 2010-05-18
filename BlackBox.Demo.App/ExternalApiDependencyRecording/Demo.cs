using System;

namespace BlackBox.Demo.App.ExternalApiDependencyRecording
{
    public class Demo
    {
        public static void Run()
        {
            var lottery = new LotteryDraw();
            int[] numbers = lottery.GenerateNumbers();

            Console.Write("Your lucky numbers are: ");
            foreach (var number in numbers)
                Console.Write(number + " ");

            Console.WriteLine(); 
        }
    }
}
