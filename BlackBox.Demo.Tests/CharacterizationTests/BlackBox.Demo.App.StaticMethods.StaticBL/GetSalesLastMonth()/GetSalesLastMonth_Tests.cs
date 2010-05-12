using BlackBox.Testing;
using Xunit;

namespace CharacterizationTests
{
	public partial class GetSalesLastMonth_Tests : CharacterizationTest
	{

		private System.Double expected;
		private System.Double actual;

		private void Run(string filename)
		{
			LoadRecording(filename);

			expected = (System.Double)GetReturnValue();
			actual = BlackBox.Demo.App.StaticMethods.StaticBL.GetSalesLastMonth();

			CompareObjects(expected, actual);
		}

		public GetSalesLastMonth_Tests()
		{
			Initialize();
		}

		[Fact]
		public void GetSalesLastMonth()
		{
			Run(@"..\..\..\BlackBox.Demo.Tests\CharacterizationTests\BlackBox.Demo.App.StaticMethods.StaticBL\GetSalesLastMonth()\GetSalesLastMonth.xml");
		}

	}
}

