using System;
using BlackBox.Demo.App.AssemblyAttribute.BL;

namespace BlackBox.Demo.App.AssemblyAttribute
{
    public class Demo
    {
        public static void Run()
        {
            Console.WriteLine("Demo of recording made on types marked using assembly attributes.");
            Console.WriteLine();

            Console.WriteLine("Name\t\tPay");
            var bl = new EmployeeBL();
            foreach(var payroll in bl.GetPayrolls())
            {
                Console.WriteLine("{0}\t{1}", payroll.Employee.Name, payroll.Pay);
            }

            Console.WriteLine();
        }
    }
}
