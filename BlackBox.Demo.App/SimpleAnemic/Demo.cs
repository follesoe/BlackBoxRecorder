using System;

namespace BlackBox.Demo.App.SimpleAnemic
{
    public class Demo
    {
        public static void Run()
        {
            Console.WriteLine("[BlackBox Recorder Demo Application]");
            Console.WriteLine();

            var bl = new EmployeeBL();
            var topPayedEmployees = bl.GetEmployeesMakingMoreThan(5000);

            Console.WriteLine("Name\t\t\tSalary");
            foreach (var employee in topPayedEmployees)
            {
                Console.WriteLine("{0}\t{1}", employee.Name, employee.Salary);
            }

            Console.WriteLine();
        }
    }
}
