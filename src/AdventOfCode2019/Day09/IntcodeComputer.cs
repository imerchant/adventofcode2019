using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2019.Day09
{
    public class IntcodeComputer
    {
        private const string OpcodeFormat = "00000";

        private readonly List<BigInteger> _outputs = new List<BigInteger>();
        private BigInteger _index = 0;
        private BigInteger _relativeBase = 0;

        public IDictionary<BigInteger, BigInteger> Program { get; }
        public BigInteger InputValue { get; }
        public IReadOnlyList<BigInteger> OutputValues => _outputs;
        public bool IsHalted { get; private set; }

        public IntcodeComputer(IList<BigInteger> program) : this(program, new BigInteger(0))
        {
        }

        public IntcodeComputer(IList<BigInteger> program, BigInteger inputValue)
        {
            Program = program
                .Select((x, i) => (Index: new BigInteger(i), Value: x))
                .ToDefaultDictionary(x => x.Index, x => x.Value, _ => new BigInteger(0));
            InputValue = inputValue;
        }

        /// <summary>
        /// Runs this IntcodeComputer to its halted state. If it takes more than 10,000 (or the given quantity) steps, halts and throws Exception.
        /// </summary>
        public IntcodeComputer RunToHalt(int maximumSteps = 10_000)
        {
            var count = 0;
            while (!IsHalted && count++ < maximumSteps)
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
                    Input(parameterModes);
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
                case InstructionType.AdjustRelativeBase:
                    AdjustRelativeBase(parameterModes);
                    break;
                default:
                    throw new Exception($"Unknown InstructionType {instruction}");
            }

            return generatedOutput;
        }

        private void Input(ParameterMode[] modes)
        {
            var storeInputIn = GetPosition(_index + 1, modes[0]);
            Program[storeInputIn] = InputValue;
            _index += 2;
        }

        private void Output(ParameterMode[] modes)
        {
            var value = Program[GetPosition(_index + 1, modes[0])];
            _outputs.Add(value);
            _index += 2;
        }

        private void Addition(ParameterMode[] modes)
        {
            var param1 = Program[GetPosition(_index + 1, modes[0])];
            var param2 = Program[GetPosition(_index + 2, modes[1])];
            var param3 = GetPosition(_index + 3, modes[2]);

            Program[param3] = param1 + param2;
            _index += 4;
        }

        private void Multiplication(ParameterMode[] modes)
        {
            var param1 = Program[GetPosition(_index + 1, modes[0])];
            var param2 = Program[GetPosition(_index + 2, modes[1])];
            var param3 = GetPosition(_index + 3, modes[2]);

            Program[param3] = param1 * param2;
            _index += 4;
        }

        private void JumpIfTrue(ParameterMode[] modes)
        {
            var param1 = Program[GetPosition(_index + 1, modes[0])];
            var param2 = Program[GetPosition(_index + 2, modes[1])];

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
            var param1 = Program[GetPosition(_index + 1, modes[0])];
            var param2 = Program[GetPosition(_index + 2, modes[1])];

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
            var param1 = Program[GetPosition(_index + 1, modes[0])];
            var param2 = Program[GetPosition(_index + 2, modes[1])];
            var param3 = GetPosition(_index + 3, modes[2]);
            var toStore = param1 < param2 ? 1 : 0;

            Program[param3] = toStore;
            _index += 4;
        }

        private void EqualOperation(ParameterMode[] modes)
        {
            var param1 = Program[GetPosition(_index + 1, modes[0])];
            var param2 = Program[GetPosition(_index + 2, modes[1])];
            var param3 = GetPosition(_index + 3, modes[2]);
            var toStore = param1 == param2 ? 1 : 0;

            Program[param3] = toStore;
            _index += 4;
        }

        private void AdjustRelativeBase(ParameterMode[] modes)
        {
            var param1 = Program[GetPosition(_index + 1, modes[0])];

            _relativeBase += param1;
            _index += 2;
        }

        /// <summary>
        /// Given the parameter's index and mode, return the index of the parameter's value.
        ///   For ParameterMode.Position:  the value at the index is an index, and is returned.
        ///   For ParameterMode.Immediate: the value at the index is the value, and parameterIndex is returned.
        ///   For ParameterMode.Relative:  the value at the index is an index adjusted by the relative base; the sum of the value and the relative base is returned.
        /// </summary>
        /// <param name="parameterIndex">The index of the parameter.</param>
        /// <param name="mode">The mode of the parameter.</param>
        /// <returns>An index position within the program.</returns>
        private BigInteger GetPosition(BigInteger parameterIndex, ParameterMode mode)
        {
            var raw = Program[parameterIndex];
            return mode switch
            {
                ParameterMode.Position => raw,
                ParameterMode.Immediate => parameterIndex,
                ParameterMode.Relative => raw + _relativeBase,
                _ => throw new Exception("Unknown parameter mode")
            };
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
            AdjustRelativeBase = 9,
            Halt = 99
        }

        private enum ParameterMode
        {
            Position = 0,
            Immediate = 1,
            Relative = 2
        }
    }
}