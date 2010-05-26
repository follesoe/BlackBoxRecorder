using System.Linq;
using BlackBox.Testing;
using Xunit;

namespace CharacterizationTests
{
	public partial class DoSomething_entity_Tests : CharacterizationTest
	{
		private BlackBox.Demo.App.InputOutputArgument.SomeEntity entityInput;
		private BlackBox.Demo.App.InputOutputArgument.SomeEntity entityOutput;

		private BlackBox.Demo.App.InputOutputArgument.SomeBL target;

		private void Run(string filename)
		{
			LoadRecording(filename);
			target = new BlackBox.Demo.App.InputOutputArgument.SomeBL();

			entityInput = (BlackBox.Demo.App.InputOutputArgument.SomeEntity)GetInputParameterValue("entity");
			entityOutput = (BlackBox.Demo.App.InputOutputArgument.SomeEntity)GetOutputParameterValue("entity");

			target.DoSomething(entityInput);

			ConfigureComparison(filename);
			CompareObjects(entityInput, entityOutput);
		}

		public DoSomething_entity_Tests()
		{
			Initialize();
		}

		protected override void ConfigureComparison(string filename)
		{
			//// Use the filename of the test to setup different
			//// comparison configurations for each test.
			//if(filename.EndsWith("DoSomething_entity.xml"))
			//{
			//    // Use IgnoreOnType to exclude a property from the comparison for all objects of that type.
			//    IgnoreOnType((BlackBox.Demo.App.InputOutputArgument.SomeEntity b) => b.Name);
			//
			//    // Use Ignore to exclude a property from the comparison for a specific instance.
			//    Ignore(expected, (BlackBox.Demo.App.InputOutputArgument.SomeEntity b) => b.Name);
			//}
		}

		[Fact]
		public void DoSomething_entity()
		{
			Run(@"..\..\..\BlackBox.Demo.Tests\CharacterizationTests\BlackBox.Demo.App.InputOutputArgument.SomeBL\DoSomething(entity)\DoSomething_entity.xml");
		}

	}
}

