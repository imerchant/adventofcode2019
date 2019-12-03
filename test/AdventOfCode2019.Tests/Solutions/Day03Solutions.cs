using AdventOfCode2019.Day03;
using AdventOfCode2019.Inputs;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2019.Tests.Solutions
{
    public class Day03Solutions : TestBase
    {
        private readonly WireGrid _wireGrid;

        public Day03Solutions(ITestOutputHelper helper) : base(helper)
        {
            _wireGrid = new WireGrid();
        }

        [Fact]
        public void Puzzle1_FindClosestIntersection()
        {
            var wires = Input.Day03Parse(Input.Day03);

            var distance = _wireGrid.FindDistanceToClosestIntersection(wires.FirstWire, wires.SecondWire);

            distance.Should().Be(2193);
        }

        [Theory]
        [InlineData("R8,U5,L5,D3", "U7,R6,D4,L4", 6)]
        [InlineData("R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83", 159)]
        [InlineData("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 135)]
        public void FindClosestIntersection(string firstWire, string secondWire, int expectedDistance)
        {
            var wireInput = $"{firstWire}\n{secondWire}";
            var wires = Input.Day03Parse(wireInput);

            _wireGrid.FindDistanceToClosestIntersection(wires.FirstWire, wires.SecondWire).Should().Be(expectedDistance);
        }
    }
}