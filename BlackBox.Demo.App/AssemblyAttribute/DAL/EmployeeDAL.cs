using System.Collections.Generic;
using BlackBox.Demo.App.AssemblyAttribute.Entity;

namespace BlackBox.Demo.App.AssemblyAttribute.DAL
{
    /*
        Dependencyt recording is enabled on DAL classes using an assembly attribute defined in AssemblyInfo.cs.
        The attribute is applied to call classes in the DAL namespace.
     
        [assembly: DependencyAttribute(
            AttributeTargetAssemblies = "BlackBox.Demo.App",
            AttributeTargetTypes = "BlackBox.Demo.App.AssemblyAttribute.DAL.*")]
     */
    public class EmployeeDAL
    {
        public List<Employee> GetAllEmployees()
        {
            return new List<Employee>
                                {
                                    new Employee {Name = "Jonas Follesø"},
                                    new Employee {Name = "Marcus Almgren"},
                                    new Employee {Name = "Alex York"}
                                };
        }
    }
}
