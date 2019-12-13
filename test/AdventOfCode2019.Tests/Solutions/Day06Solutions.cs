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

            map.Map.Should().HaveCount(12);
        }
    }
}