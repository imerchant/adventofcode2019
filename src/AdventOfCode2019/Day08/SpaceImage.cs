using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Day08
{
    public class SpaceImage : IEnumerable<Layer>
    {
        public IList<Layer> Layers { get; }

        public Layer this[int index] => Layers[index];

        public SpaceImage(string input, int width, int height)
        {
            var layerSize = width * height;
            var layerCount = input.Length / layerSize;
            Layers = new List<Layer>(layerCount);

            for (int k = 0; k < layerCount; k++)
            {
                var content = input.Skip(k * layerSize).Take(layerSize);
                Layers.Add(new Layer(content.AsString(), width, height));
            }
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
    }
}