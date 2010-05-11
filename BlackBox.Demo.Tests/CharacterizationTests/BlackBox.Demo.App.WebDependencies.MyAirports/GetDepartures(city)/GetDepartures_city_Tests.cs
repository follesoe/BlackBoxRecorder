using BlackBox.Testing;
using Xunit;

namespace CharacterizationTests
{
	public partial class GetDepartures_city_Tests : CharacterizationTest
	{
		private System.String cityInput;

		private System.String cityOutput;

		private System.Collections.Generic.List<BlackBox.Demo.App.WebDependencies.Flight> expected;
		private System.Collections.Generic.List<BlackBox.Demo.App.WebDependencies.Flight> actual;
		private BlackBox.Demo.App.WebDependencies.MyAirports target;

		private void Run(string filename)
		{
			LoadRecording(filename);
			target = new BlackBox.Demo.App.WebDependencies.MyAirports();

			cityInput = (System.String)GetInputParameterValue("city");

			cityOutput = (System.String)GetOutputParameterValue("city");

			expected = (System.Collections.Generic.List<BlackBox.Demo.App.WebDependencies.Flight>)GetReturnValue();
			actual = target.GetDepartures(cityInput);

			CompareObjects(cityInput, cityOutput);
			CompareObjects(expected, actual);
		}

		public GetDepartures_city_Tests()
		{
			Initialize();
		}

		[Fact]
		public void GetDepartures_city()
		{
			Run(@"..\..\..\BlackBox.Demo.Tests\CharacterizationTests\BlackBox.Demo.App.WebDependencies.MyAirports\GetDepartures(city)\GetDepartures_city.xml");
		}
		[Fact]
		public void GetDepartures_city_2()
		{
			Run(@"..\..\..\BlackBox.Demo.Tests\CharacterizationTests\BlackBox.Demo.App.WebDependencies.MyAirports\GetDepartures(city)\GetDepartures_city_2.xml");
		}

	}
}

