using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Day07
{
    public class IntcodeComputer
    {
        private const string OpcodeFormat = "00000";

        private readonly List<int> _outputs = new List<int>();
        private int _index = 0;

        public IList<int> Program { get; }
        public Queue<int> InputValues { get; }
        public IReadOnlyList<int> OutputValues => _outputs;
        public bool IsHalted { get; private set; }

        public IntcodeComputer(IList<int> program, params int[] inputValues)
        {
            Program = new List<int>(program);
            InputValues = new Queue<int>(inputValues);
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

        public bool Step()
        {
            var (instruction, parameterModes) = ProcessOpcode();
            var generatedOutput = false;

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
                    generatedOutput = true;
                    break;
                case InstructionType.JumpIfTrue:
                    JumpIfTrue(parameterModes);
                    break;
                case InstructionType.JumpIfFalse:
                    JumpIfFalse(parameterModes);
                    break;
                case InstructionType.LessThan:
                    LessThan(parameterModes);
                    break;
                case InstructionType.Equals:
                    EqualOperation(parameterModes);
                    break;
                default:
                    throw new Exception($"Unknown InstructionType {instruction}");
            }

            return generatedOutput;
        }

        private void Input()
        {
            var storeInputIn = Program[_index + 1];
            Program[storeInputIn] = InputValues.Dequeue();
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

        private void JumpIfTrue(ParameterMode[] modes)
        {
            var param1 = GetValue(_index + 1, modes[0]);
            var param2 = GetValue(_index + 2, modes[1]);

            if (param1 != 0)
            {
                _index = param2;
            }
            else
            {
                _index += 3;
            }
        }

        private void JumpIfFalse(ParameterMode[] modes)
        {
            var param1 = GetValue(_index + 1, modes[0]);
            var param2 = GetValue(_index + 2, modes[1]);

            if (param1 == 0)
            {
                _index = param2;
            }
            else
            {
                _index += 3;
            }
        }

        private void LessThan(ParameterMode[] modes)
        {
            var param1 = GetValue(_index + 1, modes[0]);
            var param2 = GetValue(_index + 2, modes[1]);
            var param3 = Program[_index + 3];
            var toStore = param1 < param2 ? 1 : 0;

            Program[param3] = toStore;
            _index += 4;
        }

        private void EqualOperation(ParameterMode[] modes)
        {
            var param1 = GetValue(_index + 1, modes[0]);
            var param2 = GetValue(_index + 2, modes[1]);
            var param3 = Program[_index + 3];
            var toStore = param1 == param2 ? 1 : 0;

            Program[param3] = toStore;
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

        private (InstructionType Type, ParameterMode[] Modes) ProcessOpcode()
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
            JumpIfTrue = 5,
            JumpIfFalse = 6,
            LessThan = 7,
            Equals = 8,
            Halt = 99
        }

        private enum ParameterMode
        {
            Position = 0,
            Immediate = 1
        }
    }
}