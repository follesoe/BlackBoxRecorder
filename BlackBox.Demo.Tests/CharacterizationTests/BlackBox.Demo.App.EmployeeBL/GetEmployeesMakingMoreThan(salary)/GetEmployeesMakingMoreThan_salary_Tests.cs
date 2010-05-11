using BlackBox.Demo.App.SimpleAnemic;
using BlackBox.Testing;
using Xunit;

namespace CharacterizationTests
{
	public partial class GetEmployeesMakingMoreThan_salary_Tests : CharacterizationTest
	{
		private System.Double salaryInput;

		private System.Double salaryOutput;

		private System.Collections.Generic.List<EmployeeEntity> expected;
		private System.Collections.Generic.List<EmployeeEntity> actual;
		private EmployeeBL target;

		private void Run(string filename)
		{
			LoadRecording(filename);
			target = new EmployeeBL();

			salaryInput = (System.Double)GetInputParameterValue("salary");

			salaryOutput = (System.Double)GetOutputParameterValue("salary");

			expected = (System.Collections.Generic.List<EmployeeEntity>)GetReturnValue();
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

		[Fact]
		public void GetEmployeesMakingMoreThan_salary_2()
		{
			Run(@"..\..\..\BlackBox.Demo.Tests\CharacterizationTests\BlackBox.Demo.App.EmployeeBL\GetEmployeesMakingMoreThan(salary)\GetEmployeesMakingMoreThan_salary_2.xml");
		}

	}
}

