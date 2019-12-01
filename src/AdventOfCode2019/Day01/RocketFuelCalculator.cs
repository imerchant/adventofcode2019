using System;

namespace AdventOfCode2019.Day01
{
    public class RocketFuelCalculator
    {
        /// <summary>
        /// Does a simple calculation for the fuel required for a given mass. Does not calculate additional fuel for the fuel.
        /// </summary>
        public int CalculateSimpleFuelRequired(int mass)
        {
            var fuel = (int) Math.Floor(mass / 3.0) - 2;
            return fuel >= 0 ? fuel : 0;
        }

        /// <summary>
        /// Does a complex calculation for fuel required for a given mass, including the fuel necessary for that fuel.
        /// </summary>
        public int CalculateFullFuelRequired(int startingMass)
        {
            var fuel = CalculateSimpleFuelRequired(startingMass);
            var mass = fuel;
            while (mass > 0)
            {
                mass = CalculateSimpleFuelRequired(mass);
                fuel += mass;
            }

            return fuel;
        }
    }
}