using System;
using System.Collections.Generic;
using BlackBox.Recorder;

namespace BlackBox.Demo.App.StaticMethods
{
    [Dependency]
    public class StaticDAL
    {
        public static double[] GetSales(DateTime from, DateTime to)
        {
            var timespan = to.Subtract(from);
            var random = new Random();
            var numbers = new List<double>();
            for (int i = 0; i < timespan.TotalDays; i++)
            {
                numbers.Add(random.Next(10000, 25000));
            }
            return numbers.ToArray();
        }
    }
}
