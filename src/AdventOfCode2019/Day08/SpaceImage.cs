using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Day08
{
    public class SpaceImage : IEnumerable<Layer>
    {
        public IList<Layer> Layers { get; }

        public Layer this[int index] => Layers[index];

        public string Content { get; }

        public SpaceImage(string input, int width, int height)
        {
            var layerSize = width * height;
            var layerCount = input.Length / layerSize;
            Layers = new List<Layer>(layerCount);

            for (int k = 0; k < layerCount; k++)
            {
                var layerContent = input.Skip(k * layerSize).Take(layerSize);
                Layers.Add(new Layer(layerContent.AsString(), width, height));
            }

            var content = new StringBuilder(Layers[0].Content);

            for (var k = 1; k < Layers.Count; ++k)
            {
                var layerContent = Layers[k].Content;
                for (var j = 0; j < layerContent.Length; ++j)
                {
                    if (content[j] != '2')
                        continue;
                    content[j] = layerContent[j];
                }
            }

            var allContent = content.ToString();
            content.Clear();
            for (int row = 0; row < height; row++)
            {
                var take = allContent.Skip(row * width).Take(width).ToArray();
                content.Append(take).AppendLine();
            }
            Content = content.Replace('0', ' ').Replace('1', '█').ToString();
        }

        public IEnumerator<Layer> GetEnumerator()
        {
            return Layers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Layer
    {
        public string Content { get; }
        public int Width { get; }
        public int Height { get; }

        public Layer(string content, int width, int height)
        {
            Content = content;
            Width = width;
            Height = height;
        }

        public int CountOf(char c) => Content.Count(x => x == c);
    }
}