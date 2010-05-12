using System;
using System.Linq;
using BlackBox.Recorder;

namespace BlackBox.Demo.App.StaticMethods
{
    public class StaticBL
    {
        [Recording]
        public static double GetSalesLastMonth()
        {
            return StaticDAL.GetSales(DateTime.Today.AddDays(-30), DateTime.Today).Sum();
        }
    }
}
