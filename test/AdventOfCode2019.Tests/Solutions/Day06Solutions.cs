using System.Collections.Generic;
using System.Linq;
using AdventOfCode2019.Day06;
using AdventOfCode2019.Inputs;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2019.Tests.Solutions
{
    public class Day06Solutions : TestBase
    {
        public Day06Solutions(ITestOutputHelper helper) : base(helper)
        {
        }

        [Fact]
        public void Puzzle1_CountConnections()
        {
            var orbits = Input.Day06Parse(Input.Day06);

            var map = new OrbitMap(orbits);

            map.Connections.Should().Be(139597);
        }

        [Fact]
        public void Puzzle2_CountsHopsBetweenYOUAndSAN()
        {
            var orbits = Input.Day06Parse(Input.Day06);

            var map = new OrbitMap(orbits);

            map.GetHopsBetween("SAN", "YOU").Should().Be(286);
        }

        const string Example1 =
@"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L";

        [Fact]
        public void Example1_MapsOrbitsCorrectly()
        {
            var orbits = Input.Day06Parse(Example1);

            var map = new OrbitMap(orbits);

            map.Should().HaveCount(12);
            map.Connections.Should().Be(42);
        }

        const string Example2 =
@"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L
K)YOU
I)SAN";

        [Fact]
        public void Example2_CountsHopsCorrectly()
        {
            var orbits = Input.Day06Parse(Example2);

            var map = new OrbitMap(orbits);

            map.GetHopsBetween("SAN", "YOU").Should().Be(4);
        }
    }
}