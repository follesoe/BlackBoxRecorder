using BlackBox.Testing;
using Xunit;

namespace CharacterizationTests
{
	public partial class GenerateLotteryNumbers_Tests : CharacterizationTest
	{

		private System.Int32[] expected;
		private System.Int32[] actual;
		private BlackBox.Demo.App.MultipleCallsOnDependency.LotteryDraw target;

		private void Run(string filename)
		{
			LoadRecording(filename);
			target = new BlackBox.Demo.App.MultipleCallsOnDependency.LotteryDraw();


			expected = (System.Int32[])GetReturnValue();
			actual = target.GenerateLotteryNumbers();

			CompareObjects(expected, actual);
		}

		public GenerateLotteryNumbers_Tests()
		{
			Initialize();
		}

		[Fact]
		public void GenerateLotteryNumbers()
		{
			Run(@"..\..\..\BlackBox.Demo.Tests\CharacterizationTests\BlackBox.Demo.App.MultipleCallsOnDependency.LotteryDraw\GenerateLotteryNumbers()\GenerateLotteryNumbers.xml");
		}

	}
}

