using AdventOfCode2019.Day04;
using AdventOfCode2019.Inputs;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2019.Tests.Solutions
{
    public class Day04Solutions : TestBase
    {
        private readonly PasswordGuesser _guesser;

        public Day04Solutions(ITestOutputHelper helper) : base(helper)
        {
            _guesser = new PasswordGuesser();
        }

        [Fact]
        public void Puzzle1_CountPossiblesThatMeetCriteria_WithinGivenRange()
        {
            var input = Input.Day04;

            var count = _guesser.CountNumbersThatMeetCriteria(input.Low, input.High);

            count.Should().Be(1048);
        }

        [Theory]
        [InlineData(111111, true)]
        [InlineData(223450, false)]
        [InlineData(123789, false)]
        public void IntMeetsCriteria(int possible, bool expectedMeetsCriteria)
        {
            _guesser.MeetsCriteria(possible).Should().Be(expectedMeetsCriteria);
        }
    }
}