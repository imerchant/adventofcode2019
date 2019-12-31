using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Day10
{
    public class AsteroidField : IEnumerable<Asteroid>
    {
        public IDictionary<(int X, int Y), Asteroid> Asteroids { get; }

        public Asteroid this[(int X, int Y) location] => Asteroids[location];

        public AsteroidField(IList<string> data)
        {
            Asteroids = new Dictionary<(int X, int Y), Asteroid>();

            for (int y = 0; y < data.Count; ++y)
            {
                var line = data[y];
                for (int x = 0; x < line.Length; ++x)
                {
                    if (line[x] == '#')
                        Asteroids[(x, y)] = new Asteroid(x, y);
                }
            }

            foreach (var asteroid in Asteroids.Values)
            {
                var atan2s = new HashSet<double>();
                foreach (var other in Asteroids.Keys.Where(x => x != asteroid.Location))
                {
                    atan2s.Add(Atan2(asteroid.Location, other));
                }
                asteroid.VisibleNeighbors = atan2s.Count;
            }
        }

        private double Atan2((int X, int Y) one, (int X, int Y) two)
        {
            return Math.Atan2(one.Y - two.Y, one.X - two.X);
        }

        public IEnumerator<Asteroid> GetEnumerator()
        {
            return Asteroids.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Asteroid
    {
        public (int X, int Y) Location { get; }

        public int VisibleNeighbors { get; internal set;}

        public Asteroid(int x, int y)
        {
            Location = (x, y);
        }
    }
}