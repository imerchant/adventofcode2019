using AdventOfCode2019.Day08;
using AdventOfCode2019.Inputs;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2019.Tests.Solutions
{
    public class Day08Solutions : TestBase
    {
        public Day08Solutions(ITestOutputHelper helper) : base(helper)
        {
        }

        [Fact]
        public void SpaceImage_ImportsDataCorrectly()
        {
            const string imageData = "123456789012";
            const int width = 3;
            const int height = 2;

            var image = new SpaceImage(imageData, width, height);

            image.Should().HaveCount(2);
            image[0].Content.Should().Be("123456");
            image[1].Content.Should().Be("789012");
        }
    }
}