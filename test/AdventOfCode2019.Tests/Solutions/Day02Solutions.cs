using AdventOfCode2019.Day02;
using AdventOfCode2019.Inputs;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2019.Tests.Solutions
{
    public class Day02Solutions : TestBase
    {
        public Day02Solutions(ITestOutputHelper helper) : base(helper)
        {
        }

        [Fact]
        public void Puzzle1_ValueAtPosition0_OfGravityAssistProgram()
        {
            var input = Input.Day02Parse(Input.Day02);
            input[1] = 12;
            input[2] = 2;

            var computer = new IntcodeComputer(input).RunToHalt();

            computer.Program[0].Should().Be(4576384);
        }

        [Fact]
        public void Puzzle2_FindNounAndVerb_ThatOutputs19690720()
        {
            var input = Input.Day02Parse(Input.Day02);
            int noun = 0, verb = 0;
            var found = false;

            for (noun = 0; noun < 100; ++noun)
            {
                for (verb = 0; verb < 100; ++verb)
                {
                    input[1] = noun;
                    input[2] = verb;

                    var computer = IntcodeComputer.RunToHalt(input);
                    if (computer.Program[0] == 19690720)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    break;
                }
            }

            ((100 * noun) + verb).Should().Be(5398);
        }

        [Theory]
        [InlineData("1,9,10,3,2,3,11,0,99,30,40,50", "3500,9,10,70,2,3,11,0,99,30,40,50")]
        [InlineData("1,0,0,0,99", "2,0,0,0,99")]
        [InlineData("2,3,0,3,99", "2,3,0,6,99")]
        [InlineData("2,4,4,5,99,0", "2,4,4,5,99,9801")]
        [InlineData("1,1,1,4,99,5,6,0,99", "30,1,1,4,2,5,6,0,99")]
        public void IntcodeComputer_OperatesCorrectly(string @in, string expectedOutput)
        {
            var input = Input.Day02Parse(@in);
            var expectedResult = Input.Day02Parse(expectedOutput);

            var computer = new IntcodeComputer(input);

            computer.RunToHalt();

            computer.Program.Should().BeEquivalentTo(expectedResult);
        }
    }
}