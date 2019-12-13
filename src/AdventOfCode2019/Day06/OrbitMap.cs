using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Day06
{
    public class OrbitMap : IEnumerable<Body>
    {
        public Body this[string id] => Map[id];

        public IDictionary<string, Body> Map { get; }

        public int Connections => Map.Sum(x => x.Value.Cardinality);

        public OrbitMap(IList<string> orbits)
        {
            Map = new DefaultDictionary<string, Body>(key => new Body(key));
            foreach (var orbit in orbits)
            {
                AddOrbit(orbit);
            }
        }

        public void AddOrbit(string orbit)
        {
            var split = orbit.SplitOn(')');
            var body1 = split[0];
            var body2 = split[1];

            Map[body2].Orbits.Add(Map[body1]);
            Map[body1].OrbitedBy.Add(Map[body2]);
        }

        public IEnumerator<Body> GetEnumerator()
        {
            return Map.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Body
    {
        public string Id { get; }
        public List<Body> Orbits { get; }
        public List<Body> OrbitedBy { get; }

        public int Cardinality => OrbitedBy.Sum(x => x.Cardinality) + 1;

        public Body(string id)
        {
            Id = id;
            Orbits = new List<Body>();
            OrbitedBy = new List<Body>();
        }
    }
}