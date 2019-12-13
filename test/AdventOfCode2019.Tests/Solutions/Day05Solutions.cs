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

            var computer = new IntcodeComputer(program, 1);
            computer.RunToHalt();

            computer.OutputValues.Last().Should().Be(9006673);
        }

        [Fact]
        public void Puzzle2_FindDiagnosticCode_WithInput5()
        {
            var program = Input.Day05Parse(Input.Day05);

            var computer = new IntcodeComputer(program, 5);
            computer.RunToHalt();

            computer.OutputValues.Should().HaveCount(1).And.Subject.Single().Should().Be(3629692);
        }

        const string JumpComparisonProgram = "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99";

        [Theory]
        [InlineData(6, 999)]
        [InlineData(7, 999)]
        [InlineData(8, 1000)]
        [InlineData(9, 1001)]
        [InlineData(10, 1001)]
        public void JumpComparisonProgram_OperatesCorrectly(int input, int expectedOutput)
        {
            var program = Input.Day05Parse(JumpComparisonProgram);

            var computer = new IntcodeComputer(program, input);
            computer.RunToHalt();

            computer.OutputValues.First().Should().Be(expectedOutput);
        }

        [Theory]
        [InlineData("3,9,8,9,10,9,4,9,99,-1,8", 8, 1)] // using position mode, compares input equal to 8
        [InlineData("3,9,8,9,10,9,4,9,99,-1,8", 7, 0)] // using position mode, compares input equal to 8
        [InlineData("3,9,8,9,10,9,4,9,99,-1,8", -1, 0)] // using position mode, compares input equal to 8
        [InlineData("3,9,7,9,10,9,4,9,99,-1,8", 8, 0)] // using position mode, compares input to less than 8
        [InlineData("3,9,7,9,10,9,4,9,99,-1,8", 7, 1)] // using position mode, compares input to less than 8
        [InlineData("3,9,7,9,10,9,4,9,99,-1,8", -1, 1)] // using position mode, compares input to less than 8
        [InlineData("3,3,1108,-1,8,3,4,3,99", 8, 1)] // using immediate mode, compares input equal to 8
        [InlineData("3,3,1108,-1,8,3,4,3,99", 7, 0)] // using immediate mode, compares input equal to 8
        [InlineData("3,3,1108,-1,8,3,4,3,99", -1, 0)] // using immediate mode, compares input equal to 8
        [InlineData("3,3,1107,-1,8,3,4,3,99", 8, 0)] // using immediate mode, compares input equal to 8
        [InlineData("3,3,1107,-1,8,3,4,3,99", 7, 1)] // using immediate mode, compares input to less than 8
        [InlineData("3,3,1107,-1,8,3,4,3,99", -1, 1)] // using immediate mode, compares input to less than 8
        public void ComparisonOperations_ExecuteCorrectly(string ints, int input, int expectedOutput)
        {
            var program = Input.Day05Parse(ints);

            var computer = new IntcodeComputer(program, input);
            computer.RunToHalt();

            computer.OutputValues.First().Should().Be(expectedOutput);
        }

        [Fact]
        public void IntcodeComputer_ReadsParameterModes_AndHalts()
        {
            const string input = "1002,4,3,4,33";
            var program = Input.Day05Parse(input);

            var computer = new IntcodeComputer(program, 0);
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