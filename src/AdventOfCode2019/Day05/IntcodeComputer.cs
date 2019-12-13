using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Day05
{
    public class IntcodeComputer
    {
        private const string OpcodeFormat = "00000";

        private readonly List<int> _outputs = new List<int>();
        private int _index = 0;

        public IList<int> Program { get; }
        public int InputValue { get; set; }
        public IReadOnlyList<int> OutputValues => _outputs;
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
            var (instruction, parameterModes) = FormatOpcode();

            switch (instruction)
            {
                case InstructionType.Halt:
                    IsHalted = true;
                    break;
                case InstructionType.Addition:
                    Addition(parameterModes);
                    break;
                case InstructionType.Multiplication:
                    Multiplication(parameterModes);
                    break;
                case InstructionType.Input:
                    Input();
                    break;
                case InstructionType.Output:
                    Output(parameterModes);
                    break;
            }
        }

        private void Input()
        {
            var storeInputIn = Program[_index + 1];
            Program[storeInputIn] = InputValue;
            _index += 2;
        }

        private void Output(ParameterMode[] modes)
        {
            var value = GetValue(_index + 1, modes[0]);
            _outputs.Add(value);
            _index += 2;
        }

        private void Addition(ParameterMode[] modes)
        {
            var param1 = GetValue(_index + 1, modes[0]);
            var param2 = GetValue(_index + 2, modes[1]);
            var param3 = Program[_index + 3];

            Program[param3] = param1 + param2;
            _index += 4;
        }

        private void Multiplication(ParameterMode[] modes)
        {
            var param1 = GetValue(_index + 1, modes[0]);
            var param2 = GetValue(_index + 2, modes[1]);
            var param3 = Program[_index + 3];

            Program[param3] = param1 * param2;
            _index += 4;
        }

        private int GetValue(int parameterIndex, ParameterMode mode)
        {
            var raw = Program[parameterIndex];
            switch (mode)
            {
                case ParameterMode.Immediate:
                    return raw;
                case ParameterMode.Position:
                    return Program[raw];
                default:
                    throw new Exception("Unknown parameter mode");
            }

        }

        private (InstructionType Type, ParameterMode[] Modes) FormatOpcode()
        {
            var opcodeWithModes = Program[_index].ToString(OpcodeFormat);
            var opcode = int.Parse(opcodeWithModes.Substring(3));
            if (!Enum.IsDefined(typeof(InstructionType), opcode))
            {
                throw new Exception($"Could not parse opcode {Program[_index]}");
            }
            var type = (InstructionType) opcode;
            var modes = opcodeWithModes.Substring(0, 3).Reverse().Select(x => (ParameterMode)(x - '0')).ToArray();
            return (type, modes);
        }

        private enum InstructionType
        {
            Addition = 1,
            Multiplication = 2,
            Input = 3,
            Output = 4,
            Halt = 99
        }

        private enum ParameterMode
        {
            Position = 0,
            Immediate = 1
        }
    }
}