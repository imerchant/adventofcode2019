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
            var permutations = Input.Day07Permutations.ZeroToFour;

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

        [Fact]
        public void Puzzle2_FindHighestOutputWithFeedbackLoop()
        {
            var program = Input.Day07Parse(Input.Day07);
            var permutations = Input.Day07Permutations.FiveToNine;

            var stopwatch = Stopwatch.StartNew();
            var allAmplifierRuns = permutations.Select(x =>
            {
                var amplifiers = new Amplifiers(program, x);
                amplifiers.GetSignalWithFeedbackLoops();
                return amplifiers;
            }).ToList();
            stopwatch.Stop();

            Output.WriteLine(stopwatch.ElapsedMilliseconds.ToString());

            allAmplifierRuns.OrderByDescending(x => x.OutputToThrusters).First().OutputToThrusters.Should().Be(4275738);
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

        [Theory]
        [MemberData(nameof(ExampleFeedback_ProgramsAndPhaseSettingsCases))]
        public void ExampleFeedback_ProgramsAndPhaseSettings(string program, string phaseSettings, int expectedResult)
        {
            var amplifiers = new Amplifiers(Input.Day07Parse(program), phaseSettings);

            amplifiers.GetSignalWithFeedbackLoops().Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> ExampleFeedback_ProgramsAndPhaseSettingsCases()
        {
            const string Program1 = "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5";
            const string Program2 = "3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10";

            yield return new object[] { Program1, "98765", 139629729 };
            yield return new object[] { Program2, "97856", 18216 };
        }

        [Fact]
        public void ControlProgram_GeneratesOutputOnStep()
        {
            var computer = new IntcodeComputer(Input.Day07Parse(Input.Day07), 5, 0);
            var steps = 0;
            do
            {
                steps++;
            }
            while (!computer.Step());
        }
    }
}