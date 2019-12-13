using System.Linq;
using AdventOfCode2019.Day05;
using AdventOfCode2019.Inputs;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2019.Tests.Solutions
{
    public class Day05Solutions : TestBase
    {
        public Day05Solutions(ITestOutputHelper helper) : base(helper)
        {
        }

        [Fact]
        public void Puzzle1_WhenProgramPassesAllTests_FindFinalDiagnosticCode()
        {
            var program = Input.Day05Parse(Input.Day05);

            var computer = new IntcodeComputer(program);
            computer.InputValue = 1;
            computer.RunToHalt();

            computer.OutputValues.Last().Should().Be(9006673);
        }

        [Fact]
        public void IntcodeComputer_ReadsParameterModes_AndHalts()
        {
            const string input = "1002,4,3,4,33";
            var program = Input.Day05Parse(input);

            var computer = new IntcodeComputer(program);
            computer.Step();
            computer.Step();

            computer.IsHalted.Should().BeTrue();
        }

        [Theory]
        [InlineData(10002, "10002")]
        [InlineData(2, "00002")]
        [InlineData(102, "00102")]
        [InlineData(99, "00099")]
        [InlineData(101, "00101")]
        public void OpcodeFormat_FormatsAsExpected(int input, string expectedFormatResult)
        {
            const string format = "00000";
            input.ToString(format).Should().Be(expectedFormatResult);
        }
    }
}