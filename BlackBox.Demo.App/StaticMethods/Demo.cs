using System;

namespace BlackBox.Demo.App.StaticMethods
{
    public class Demo
    {
        public static void Run()
        {
            double sales = StaticBL.GetSalesLastMonth();
            Console.WriteLine("Sales last 30 days: {0}", sales);
            Console.WriteLine();
        }
    }
}
