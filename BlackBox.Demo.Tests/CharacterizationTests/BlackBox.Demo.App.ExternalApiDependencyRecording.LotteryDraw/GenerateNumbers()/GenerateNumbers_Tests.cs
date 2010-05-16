using BlackBox.Testing;
using Xunit;

namespace CharacterizationTests
{
	public partial class GenerateNumbers_Tests : CharacterizationTest
	{

		private System.Int32[] expected;
		private System.Int32[] actual;
		private BlackBox.Demo.App.ExternalApiDependencyRecording.LotteryDraw target;

		private void Run(string filename)
		{
			LoadRecording(filename);
			target = new BlackBox.Demo.App.ExternalApiDependencyRecording.LotteryDraw();


			expected = (System.Int32[])GetReturnValue();
			actual = target.GenerateNumbers();

			CompareObjects(expected, actual);
		}

		public GenerateNumbers_Tests()
		{
			Initialize();
		}

		[Fact]
		public void GenerateNumbers()
		{
			Run(@"..\..\..\BlackBox.Demo.Tests\CharacterizationTests\BlackBox.Demo.App.ExternalApiDependencyRecording.LotteryDraw\GenerateNumbers()\GenerateNumbers.xml");
		}

	}
}

