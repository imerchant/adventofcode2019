using System;
using System.Collections.Generic;

namespace AdventOfCode2019.Day02
{
    public class IntcodeComputer
    {
        private int _index = 0;

        public IList<int> Program { get; }
        public bool IsHalted { get; private set; }

        public IntcodeComputer(IList<int> program)
        {
            Program = new List<int>(program);
        }

        /// <summary>
        /// Runs this IntcodeComputer to its halted state. If it takes more than 10,000 steps, halts and throws Exception.
        /// </summary>
        public IntcodeComputer RunToHalt()
        {
            var count = 0;
            while (!IsHalted && count++ < 10_000)
            {
                Step();
            }

            if (!IsHalted)
            {
                throw new Exception("Ran for 10,000 steps and did not halt");
            }

            return this;
        }

        public void Step()
        {
            var opcode = Program[_index];
            if (opcode == 99) // program is to halt
            {
                IsHalted = true;
                return;
            }
            else if (opcode != 1 && opcode != 2) // unexpected operation
            {
                throw new Exception($"Unknown opcode {opcode} at position {_index}.");
            }

            var position1 = Program[_index + 1];
            var position2 = Program[_index + 2];
            var position3 = Program[_index + 3];
            if (opcode == 1) // addition
            {
                Program[position3] = Program[position1] + Program[position2];
            }
            else if (opcode == 2) // multiplication
            {
                Program[position3] = Program[position1] * Program[position2];
            }
            else // probably unnecessary
            {
                throw new Exception($"Unknown opcode {opcode} at position {_index}.");
            }

            _index += 4;
        }

        /// <summary>
        /// Creates a new IntcodeComputer with the given input and runs it to halt.
        /// </summary>
        public static IntcodeComputer RunToHalt(IList<int> input)
        {
            var computer = new IntcodeComputer(input);
            computer.RunToHalt();
            return computer;
        }
    }
}