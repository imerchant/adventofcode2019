using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode2019.Day07;
using AdventOfCode2019.Inputs;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2019.Tests.Solutions
{
    public class Day07Solutions : TestBase
    {
        public Day07Solutions(ITestOutputHelper helper) : base(helper)
        {
        }

        [Fact]
        public void Puzzle1_FindHighestOutput()
        {
            var program = Input.Day07Parse(Input.Day07);
            var permutations = Input.Day07Permutations;

            var stopwatch = Stopwatch.StartNew();
            var allAmplifierRuns = permutations.Select(x =>
            {
                var amplifiers = new Amplifiers(program, x);
                amplifiers.GetSignalForThrusters();
                return amplifiers;
            }).ToList();
            stopwatch.Stop();

            Output.WriteLine(stopwatch.ElapsedMilliseconds.ToString());

            allAmplifierRuns.OrderByDescending(x => x.OutputToThrusters).First().OutputToThrusters.Should().Be(95757);
        }

        [Theory]
        [MemberData(nameof(ExampleProgramsAndPhaseSettingsCases))]
        public void ExampleProgramsAndPhaseSettings(string program, string phaseSettings, int expectedResult)
        {
            var amplifiers = new Amplifiers(Input.Day07Parse(program), phaseSettings);

            amplifiers.GetSignalForThrusters().Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> ExampleProgramsAndPhaseSettingsCases()
        {
            const string Program1 = "3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0";
            const string Program2 = "3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0";
            const string Program3 = "3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0";

            yield return new object[] { Program1, "43210", 43210 };
            yield return new object[] { Program2, "01234", 54321 };
            yield return new object[] { Program3, "10432", 65210 };
        }
    }
}