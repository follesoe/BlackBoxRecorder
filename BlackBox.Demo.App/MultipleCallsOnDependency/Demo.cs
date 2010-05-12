using System;

namespace BlackBox.Demo.App.MultipleCallsOnDependency
{
    public class Demo
    {
        public static void Run()
        {
            var lottery = new LotteryDraw();
            int[] numbers = lottery.GenerateLotteryNumbers();

            Console.Write("Your lucky numbers are: ");
            foreach(var number in numbers)
                Console.Write(number + " ");

            Console.WriteLine();
        }
    }
}
