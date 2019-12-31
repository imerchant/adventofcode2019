using System;
using System.Linq;
using AdventOfCode2019.Day10;
using AdventOfCode2019.Inputs;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2019.Tests.Solutions
{
    public class Day10Solutions : TestBase
    {
        public Day10Solutions(ITestOutputHelper helper) : base(helper)
        {
        }

        [Fact]
        public void Puzzle1_FindBestLocation()
        {
            var field = new AsteroidField(Input.Day10Parse(Input.Day10));

            var bestLocation = field.OrderByDescending(x => x.VisibleNeighbors).First();

            bestLocation.VisibleNeighbors.Should().Be(329);
        }

        [Theory]
        [InlineData(Example1, 3, 4, 8)]
        [InlineData(Example2, 5, 8, 33)]
        [InlineData(Example3, 1, 2, 35)]
        [InlineData(Example4, 6, 3, 41)]
        [InlineData(Example5, 11, 13, 210)]
        public void Examples_FindBestLocation(string data, int expectedX, int expectedY, int expectedNeighbors)
        {
            var field = new AsteroidField(Input.Day10Parse(data));

            var bestLocation = field.OrderByDescending(x => x.VisibleNeighbors).First();
            bestLocation.Location.Should().Be((expectedX, expectedY));
            bestLocation.VisibleNeighbors.Should().Be(expectedNeighbors);
        }

        [Theory]
        [InlineData(Example1, 3, 4)]
        [InlineData(Example2, 5, 8)]
        [InlineData(Example3, 1, 2)]
        [InlineData(Example4, 6, 3)]
        [InlineData(Example5, 11, 13)]
        public void AsteroidField_ParsesAsteroidsCorrectly(string data, int x, int y)
        {
            var expectedCount = data.Count(x => x == '#');

            var field = new AsteroidField(Input.Day10Parse(data));

            field.Should().HaveCount(expectedCount);
            field.Asteroids.TryGetValue((x, y), out _).Should().BeTrue();
        }

        public const string Example1 =
@".#..#
.....
#####
....#
...##";

        public const string Example2 =
@"......#.#.
#..#.#....
..#######.
.#.#.###..
.#..#.....
..#....#.#
#..#....#.
.##.#..###
##...#..#.
.#....####";

        public const string Example3 =
@"#.#...#.#.
.###....#.
.#....#...
##.#.#.#.#
....#.#.#.
.##..###.#
..#...##..
..##....##
......#...
.####.###.";

        public const string Example4 =
@".#..#..###
####.###.#
....###.#.
..###.##.#
##.##.#.#.
....###..#
..#.#..#.#
#..#.#.###
.##...##.#
.....#.#..";

        public const string Example5 =
@".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##";

        [Fact]
        public void Atan2_SanityChecking()
        {
            // ....(0,5)..(0,12)
            // .....1......2
            // .....#......#
            var atan2to1 = Math.Atan2(0, 12 - 5);
            var atan1to2 = Math.Atan2(0, 5 - 12);

            atan2to1.Should().NotBe(atan1to2);

            // ....(0,5)..(0,12)..(0, 20)
            // .....1......2.......3
            // .....#......#.......#

            var atan1to3 = Math.Atan2(0, 5 - 20);
            atan1to3.Should().Be(atan1to2);
        }
    }
}