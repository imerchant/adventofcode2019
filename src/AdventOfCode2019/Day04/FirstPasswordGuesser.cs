using System.Linq;

namespace AdventOfCode2019.Day04
{
    public class FirstPasswordGuesser
    {
        public int CountNumbersThatMeetCriteria(int low, int high)
        {
            var count = 0;
            for (int k = low; k < high; k++)
            {
                if (MeetsCriteria(k))
                {
                    count++;
                }
            }

            return count;
        }

        public bool MeetsCriteria(int num)
        {
            var text = num.ToString();

            if (text.Length != 6) // check number is correct number of digits
            {
                return false;
            }

            if (text.Distinct().Count() > 5) // check for repeated digits
            {
                return false;
            }

            if (text != text.OrderBy(x => x).AsString()) // check digits are in increasing order
            {
                return false;
            }

            return true;
        }
    }
}