using System.Linq;
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
        public void Puzzle1_LayerWithLeast0s()
        {
            const int width = 25;
            const int height = 6;
            var image = new SpaceImage(Input.Day08, width, height);

            var lowest0Layer = image.OrderBy(x => x.CountOf('0')).First();

            (lowest0Layer.CountOf('1') * lowest0Layer.CountOf('2')).Should().Be(1206);
        }

        [Fact]
        public void Puzzle2_SteganographicMessageInImage()
        {
            const int width = 25;
            const int height = 6;

            var image = new SpaceImage(Input.Day08, width, height);

            Output.WriteLine(string.Empty);
            Output.WriteLine(image.Content);

            // Produces: EJRGP
            // ████   ██ ███   ██  ███  
            // █       █ █  █ █  █ █  █ 
            // ███     █ █  █ █    █  █ 
            // █       █ ███  █ ██ ███  
            // █    █  █ █ █  █  █ █    
            // ████  ██  █  █  ███ █    
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
            image[0].Width.Should().Be(width);
            image[0].Height.Should().Be(height);
            image[1].Content.Should().Be("789012");
        }
    }
}