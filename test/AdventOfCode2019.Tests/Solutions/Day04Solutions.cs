using AdventOfCode2019.Day04;
using AdventOfCode2019.Inputs;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2019.Tests.Solutions
{
    public class Day04Solutions : TestBase
    {
        private readonly FirstPasswordGuesser _firstGuesser;
        private readonly SecondPasswordGuesser _secondGuesser;

        public Day04Solutions(ITestOutputHelper helper) : base(helper)
        {
            _firstGuesser = new FirstPasswordGuesser();
            _secondGuesser = new SecondPasswordGuesser();
        }

        [Fact]
        public void Puzzle1_CountPossiblesThatMeetFirstCriteria_WithinGivenRange()
        {
            var input = Input.Day04;

            var count = _firstGuesser.CountNumbersThatMeetCriteria(input.Low, input.High);

            count.Should().Be(1048);
        }

        [Fact]
        public void Puzzle2_CountPossiblesThatMeetSecondCriteria_WithinGivenRange()
        {
            var input = Input.Day04;

            var count = _secondGuesser.CountNumbersThatMeetCriteria(input.Low, input.High);

            count.Should().HaveCount(677);
        }

        [Theory]
        [InlineData(111111, true)]
        [InlineData(223450, false)]
        [InlineData(123789, false)]
        public void IntMeetsCriteria(int possible, bool expectedMeetsCriteria)
        {
            _firstGuesser.MeetsCriteria(possible).Should().Be(expectedMeetsCriteria);
        }

        [Theory]
        [InlineData(112233, true)]
        [InlineData(123444, false)]
        [InlineData(111122, true)]
        [InlineData(121345, false)]
        [InlineData(123456, false)]
        public void IntMeetsSecondCriteria(int possible, bool expectedMeetsCriteria)
        {
            _secondGuesser.MeetsCriteria(possible).Should().Be(expectedMeetsCriteria);
        }
    }
}