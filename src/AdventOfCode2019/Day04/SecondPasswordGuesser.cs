using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Day04
{
    public class SecondPasswordGuesser
    {
        public IList<int> CountNumbersThatMeetCriteria(int low, int high)
        {
            var list = new List<int>();
            for (int k = low; k < high; k++)
            {
                if (MeetsCriteria(k))
                {
                    list.Add(k);
                }
            }

            return list;
        }

        public bool MeetsCriteria(int possible)
        {
            var text = possible.ToString();

            if (text.Length != 6) // check number is correct number of digits
            {
                return false;
            }

            // check this first so we don't count something like "121345" as correct
            if (text != text.OrderBy(x => x).AsString()) // check digits are in increasing order
            {
                return false;
            }

            if (!DuplicatesExistAndAreInGroupsOf2()) // new duplicate rule
            {
                return false;
            }

            return true;

            bool DuplicatesExistAndAreInGroupsOf2()
            {
                var duplicates = new Dictionary<char, int>();
                foreach (var digit in text.Distinct())
                {
                    var count = text.Count(x => x == digit);
                    if (count != 1)
                    {
                        duplicates.Add(digit, count);
                    }
                }

                if (!duplicates.Any())
                {
                    return false;
                }

                return duplicates.Any(x => x.Value == 2);
            }
        }
    }
}