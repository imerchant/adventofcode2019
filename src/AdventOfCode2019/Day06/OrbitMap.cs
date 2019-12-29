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

        public int GetHopsBetween(string id1, string id2)
        {
            var body1 = Map[id1];
            var body2 = Map[id2];

            var path1 = GetPath(body1);
            var path2 = GetPath(body2);

            var shared = path1.Intersect(path2).ToList();

            var uniques = new HashSet<Body>(path1);
            uniques.UnionWith(path2);
            uniques.ExceptWith(shared);

            return uniques.Count;

            Stack<Body> GetPath(Body body)
            {
                var stack = new Stack<Body>();
                var thisBody = body.Orbits.First();
                do
                {
                    stack.Push(thisBody);
                    thisBody = thisBody.Orbits.FirstOrDefault();
                }
                while (thisBody != null);

                return stack;
            }
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

        public int Cardinality => OrbitedBy.Count + OrbitedBy.Sum(x => x.Cardinality);

        public Body(string id)
        {
            Id = id;
            Orbits = new List<Body>();
            OrbitedBy = new List<Body>();
        }

        public override string ToString() => Id;

        public override bool Equals(object obj) => Equals(obj as Body);

        public bool Equals(Body body)
        {
            if (body == null) return false;
            if (ReferenceEquals(this, body)) return true;
            return body.Id == Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}