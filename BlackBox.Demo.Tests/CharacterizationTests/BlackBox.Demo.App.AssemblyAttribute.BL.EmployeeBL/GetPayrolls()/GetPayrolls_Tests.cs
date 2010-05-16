using BlackBox.Testing;
using Xunit;

namespace CharacterizationTests
{
	public partial class GetPayrolls_Tests : CharacterizationTest
	{

		private System.Collections.Generic.List<BlackBox.Demo.App.AssemblyAttribute.Entity.Payroll> expected;
		private System.Collections.Generic.List<BlackBox.Demo.App.AssemblyAttribute.Entity.Payroll> actual;
		private BlackBox.Demo.App.AssemblyAttribute.BL.EmployeeBL target;

		private void Run(string filename)
		{
			LoadRecording(filename);
			target = new BlackBox.Demo.App.AssemblyAttribute.BL.EmployeeBL();


			expected = (System.Collections.Generic.List<BlackBox.Demo.App.AssemblyAttribute.Entity.Payroll>)GetReturnValue();
			actual = target.GetPayrolls();

			CompareObjects(expected, actual);
		}

		public GetPayrolls_Tests()
		{
			Initialize();
		}

		[Fact]
		public void GetPayrolls()
		{
			Run(@"..\..\..\BlackBox.Demo.Tests\CharacterizationTests\BlackBox.Demo.App.AssemblyAttribute.BL.EmployeeBL\GetPayrolls()\GetPayrolls.xml");
		}

	}
}

