using System.Linq;
using System.Collections.Generic;
using BlackBox.Recorder;

namespace BlackBox.Demo.App
{
    public class EmployeeBL
    {
        [Recording]
        public List<EmployeeEntity> GetEmployeesMakingMoreThan(double salary)
        {
            var dal = new EmployeeDAL();

            var employees = dal.GetAllEmployees();

            return employees.Where(e => e.Salary > salary).ToList();
        }

    }
}
