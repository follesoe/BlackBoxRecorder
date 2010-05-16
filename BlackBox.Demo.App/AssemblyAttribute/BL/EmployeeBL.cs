using System.Collections.Generic;
using BlackBox.Demo.App.AssemblyAttribute.DAL;
using BlackBox.Demo.App.AssemblyAttribute.Entity;

namespace BlackBox.Demo.App.AssemblyAttribute.BL
{
    /*
        Recording is enabled on BL classes using an assembly attribute defined in AssemblyInfo.cs.
        The attribute is applied to call classes in the BL namespace.
     
        [assembly: RecordingAttribute(
            AttributeTargetAssemblies = "BlackBox.Demo.App",
            AttributeTargetTypes = "BlackBox.Demo.App.AssemblyAttribute.BL.*")]
     */
    public class EmployeeBL
    {
        public List<Payroll> GetPayrolls()
        {
            var dal = new EmployeeDAL();
            var employees = dal.GetAllEmployees();

            var payrolls = new List<Payroll>();

            foreach (var employee in employees)
            {
                var payroll = new Payroll {Employee = employee, Pay = 100000.0};
                payrolls.Add(payroll);
            }
            return payrolls;
        }
    }
}
