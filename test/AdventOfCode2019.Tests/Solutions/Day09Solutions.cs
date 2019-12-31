using System.Linq;
using System.Numerics;
using AdventOfCode2019.Day09;
using AdventOfCode2019.Inputs;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2019.Tests.Solutions
{
    public class Day09Solutions : TestBase
    {
        public Day09Solutions(ITestOutputHelper helper) : base(helper)
        {
        }

        [Fact]
        public void Puzzle1_ProgramReturnsNoOpcodesAsMalfunctioning_AndBOOSTKeycode()
        {
            var program = Input.Day09Parse(Input.Day09);
            var computer = new IntcodeComputer(program, new BigInteger(1));

            computer.RunToHalt();

            computer.OutputValues.Should().BeEquivalentTo(new [] { new BigInteger(3598076521) });
        }

        [Fact(Skip = "Adds about half a second to entire test suite runtime")]
        public void Puzzle2_RunProgramWithInput2_WaitForResults()
        {
            var program = Input.Day09Parse(Input.Day09);
            var computer = new IntcodeComputer(program, new BigInteger(2));

            computer.RunToHalt(1_000_000);

            computer.OutputValues.Should().BeEquivalentTo(new [] { new BigInteger(90722) });
        }

        [Fact]
        public void IntcodeComputer_ReturnsQuine()
        {
            const string input = "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99";
            var program = Input.Day09Parse(input);
            var computer = new IntcodeComputer(program);

            computer.RunToHalt();

            computer.OutputValues.Should().BeEquivalentTo(program);
        }

        [Fact]
        public void IntcodeComputer_ReturnsLargeNumberFromProgramInput()
        {
            const string input = "104,1125899906842624,99";
            var program = Input.Day09Parse(input);
            var computer = new IntcodeComputer(program);

            computer.RunToHalt();

            computer.OutputValues.First().Should().Be(new BigInteger(1125899906842624));
        }

        [Fact]
        public void IntcodeComputer_Returns16DigitNumber()
        {
            const string input = "1102,34915192,34915192,7,4,7,99,0";
            var program = Input.Day09Parse(input);
            var computer = new IntcodeComputer(program);

            computer.RunToHalt();

            computer.OutputValues.First().ToString().Should().HaveLength(16);
        }

        [Theory]
        [InlineData("109, -1, 4, 1, 99", -1)]
        [InlineData("109, -1, 104, 1, 99", 1)]
        [InlineData("109, -1, 204, 1, 99", 109)]
        [InlineData("109, 1, 9, 2, 204, -6, 99", 204)]
        [InlineData("109, 1, 109, 9, 204, -6, 99", 204)]
        [InlineData("109, 1, 209, -1, 204, -106, 99", 204)]
        public void IntcodeComputer_UsesRelativeModeCorrectly(string input, int output)
        {
            var program = Input.Day09Parse(input);
            var computer = new IntcodeComputer(program);

            computer.RunToHalt();

            computer.OutputValues.Should().HaveCount(1).And.BeEquivalentTo(new [] { new BigInteger(output) });
        }

        [Theory]
        [InlineData("109, 1, 3, 3, 204, 2, 99")]
        [InlineData("109, 1, 203, 2, 204, 2, 99")]
        public void IntcodeComputer_ReadsInputAndRelativeModeCorrectly(string input)
        {
            var program = Input.Day09Parse(input);
            var inputValues = new [] { new BigInteger(-99) };
            var computer = new IntcodeComputer(program, inputValues.First());

            computer.RunToHalt();

            computer.OutputValues.Should().HaveCount(1).And.BeEquivalentTo(inputValues);
        }

        [Fact]
        public void IntcodeComputer_AddsWithRelativeModeCorrectly()
        {
            const string input = "109, 1, 21101, 1, 1, 4, 204, 4, 99";
            var program = Input.Day09Parse(input);
            var computer = new IntcodeComputer(program);

            computer.RunToHalt();

            computer.OutputValues.Should().BeEquivalentTo(new [] { new BigInteger(2) });
        }
    }
}