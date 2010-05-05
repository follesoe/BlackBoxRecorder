using BlackBox;
using BlackBox.Testing;
using Xunit;

namespace CharacterizationTests
{
	
	public partial class GetEmployeesMakingMoreThan_salary_Tests : CharacterizationTest
	{
		private System.Double salaryInput;

		private System.Double salaryOutput;

		private System.Collections.Generic.List<BlackBox.Demo.App.EmployeeEntity> expected;
		private System.Collections.Generic.List<BlackBox.Demo.App.EmployeeEntity> actual;
		private BlackBox.Demo.App.EmployeeBL target;

		private void Run(string filename)
		{
			LoadRecording(filename);
			target = new BlackBox.Demo.App.EmployeeBL();

			salaryInput = (System.Double)GetInputParameterValue("salary");

			salaryOutput = (System.Double)GetOutputParameterValue("salary");

			expected = (System.Collections.Generic.List<BlackBox.Demo.App.EmployeeEntity>)GetReturnValue();
			actual = target.GetEmployeesMakingMoreThan(salaryInput);

			CompareObjects(salaryInput, salaryOutput);
			CompareObjects(expected, actual);
		}

		public GetEmployeesMakingMoreThan_salary_Tests()
		{
			Initialize();
		}

		[Fact]
		public void GetEmployeesMakingMoreThan_salary()
		{
			Run(@"..\..\..\BlackBox.Demo.Tests\CharacterizationTests\BlackBox.Demo.App.EmployeeBL\GetEmployeesMakingMoreThan(salary)\GetEmployeesMakingMoreThan_salary.xml");
		}

	}
}

