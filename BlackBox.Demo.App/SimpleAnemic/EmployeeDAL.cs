using System.Collections.Generic;

using BlackBox.Recorder;

namespace BlackBox.Demo.App.SimpleAnemic
{
    [Dependency]
    public class EmployeeDAL
    {
        public List<EmployeeEntity> GetAllEmployees()
        {
            var employees = new List<EmployeeEntity>();
            for(int i = 0; i < 10; i++)
            {
                var employee = new EmployeeEntity();
                employee.Id = i;
                employee.Salary = (i + 1.0)*1000;
                employee.Name = "Employee number " + i;
                employees.Add(employee);
            }
            return employees;
        }
    }
}
