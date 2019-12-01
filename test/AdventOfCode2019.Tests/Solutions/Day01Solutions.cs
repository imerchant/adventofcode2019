using System.Linq;
using AdventOfCode2019.Day01;
using AdventOfCode2019.Inputs;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2019.Tests.Solutions
{
    public class Day01Solutions : TestBase
    {
        private static RocketFuelCalculator _calculator;

        public Day01Solutions(ITestOutputHelper helper) : base(helper)
        {
            _calculator = new RocketFuelCalculator();
        }

        [Fact]
        public void Puzzle1_CalculateSimpleFuelRequirements()
        {
            var masses = Input.Day01Parse(Input.Day01);

            var fuel = masses.Sum(_calculator.CalculateSimpleFuelRequired);

            fuel.Should().Be(3464735);
        }

        [Fact]
        public void Puzzle2_CalculateFullFuelRequirements()
        {
            var masses = Input.Day01Parse(Input.Day01);

            var fuel = masses.Sum(_calculator.CalculateFullFuelRequired);

            fuel.Should().Be(5194211);
        }

        [Theory]
        [InlineData(12, 2)]
        [InlineData(14, 2)]
        [InlineData(1969, 966)]
        [InlineData(100756, 50346)]
        public void RocketFuelCalculator_CalculatesFullCorrectly(int mass, int expectedFuel)
        {
            _calculator.CalculateFullFuelRequired(mass).Should().Be(expectedFuel);
        }

        [Theory]
        [InlineData(12, 2)]
        [InlineData(14, 2)]
        [InlineData(1969, 654)]
        [InlineData(100756, 33583)]
        public void RocketFuelCalculator_CalculatesSimpleCorrectly(int mass, int expectedFuel)
        {
            _calculator.CalculateSimpleFuelRequired(mass).Should().Be(expectedFuel);
        }
    }
}