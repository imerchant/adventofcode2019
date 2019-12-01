using System;

namespace AdventOfCode2019.Day01
{
    public class RocketFuelCalculator
    {
        public int CalculateFuelRequired(int mass)
        {
            return (int) Math.Floor(mass / 3.0) - 2;
        }
    }
}