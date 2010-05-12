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

			CompareObjects(entityInput, entityOutput);
		}

		public DoSomething_entity_Tests()
		{
			Initialize();
		}

		[Fact]
		public void DoSomething_entity()
		{
			Run(@"..\..\..\BlackBox.Demo.Tests\CharacterizationTests\BlackBox.Demo.App.InputOutputArgument.SomeBL\DoSomething(entity)\DoSomething_entity.xml");
		}

	}
}

