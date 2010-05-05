using System;
using BlackBox.CodeGeneration;

namespace BlackBox.Demo.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigreBlackBox();
            Console.WriteLine("[BlackBox Recorder Demo Application]");
            Console.WriteLine();

            var bl = new EmployeeBL();
            var topPayedEmployees = bl.GetEmployeesMakingMoreThan(5000);

            Console.WriteLine("Name\t\t\tSalary");
            foreach(var employee in topPayedEmployees)
            {
                Console.WriteLine("{0}\t{1}", employee.Name, employee.Salary);
            }

            Console.ReadLine();
        }

        private static void ConfigreBlackBox()
        {
            Configuration.OutputDirectory = @"..\..\..\BlackBox.Demo.Tests\";
            Configuration.TestFlavour = new XunitFlavour();
        }
    }
}
