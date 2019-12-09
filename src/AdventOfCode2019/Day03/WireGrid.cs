using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Day03
{
    public class WireGrid
    {
        public int FindDistanceToClosestIntersection(IList<string> firstWire, IList<string> secondWire)
        {
            var first = GetWirePresentAt(firstWire);
            var second = GetWirePresentAt(secondWire);

            var wiresCrossAt = first.Intersect(second).OrderBy(x => ManhattanDistance(x));
            return ManhattanDistance(wiresCrossAt.First());
        }

        public IList<(int X, int Y, int FirstWireSteps, int SecondWireSteps, int CombinedSteps)> FindSteps(IList<string> firstWire, IList<string> secondWire)
        {
            var first = GetWirePresentAt(firstWire).ToList();
            var second = GetWirePresentAt(secondWire).ToList();

            var intersections = first.Intersect(second);

            var data = from intersection in intersections
                let firstWireSteps = first.IndexOf(intersection) + 1 // +1 for each since (0,0) isn't in the collection
                let secondWireSteps = second.IndexOf(intersection) + 1
                select (intersection.X, intersection.Y, firstWireSteps, secondWireSteps, firstWireSteps + secondWireSteps);

            return data.OrderBy(x => x.Item5).ToList();
        }

        private static IList<(int X, int Y)> GetWirePresentAt(IList<string> wire)
        {
            var wirePresentAt = new List<(int X, int Y)>();
            var last = (0, 0);
            foreach (var directionAndDistance in wire)
            {
                var marked = GetCoords(last, directionAndDistance).ToList();
                last = marked.Last();
                wirePresentAt.AddRange(marked);
            }

            return wirePresentAt;
        }

        private static IEnumerable<(int, int)> GetCoords((int X, int Y) starting, string directionAndDistance)
        {
            var (direction, distance) = Parse(directionAndDistance);

            var coords = new HashSet<(int, int)>();
            var x = starting.X;
            var y = starting.Y;
            for (var k = 0; k < distance; ++k)
            {
                switch(direction)
                {
                    case Direction.U:
                        ++y;
                        break;
                    case Direction.R:
                        ++x;
                        break;
                    case Direction.D:
                        --y;
                        break;
                    case Direction.L:
                        --x;
                        break;
                }

                coords.Add((x, y));
            }

            return coords;
        }

        private static (Direction Direction, int Distance) Parse(string distance)
        {
            var direction = distance[0].ToString().ParseEnum<Direction>();
            return (direction, int.Parse(distance.Substring(1)));
        }

        private static int ManhattanDistance((int, int) ints)
        {
            return Math.Abs(ints.Item1) + Math.Abs(ints.Item2);
        }

        private enum Direction
        {
            U, R, D, L
        }
    }
}