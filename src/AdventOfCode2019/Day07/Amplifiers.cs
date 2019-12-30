using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Day07
{
    public class Amplifiers
    {
        private const string AmplifierIds = "ABCDE";
        private readonly IList<int> _controllerSoftware;
        private readonly IDictionary<char, int> _phaseSettings;

        public int OutputToThrusters { get; private set; }

        public Amplifiers(IList<int> controllerSoftware, string phaseSettings)
        {
            _controllerSoftware = controllerSoftware;
            _phaseSettings = AmplifierIds.Select((x, i) => (x, phaseSettings[i] - '0')).ToDictionary(x => x.Item1, x => x.Item2);
        }

        public int GetSignalForThrusters()
        {
            var lastOutput = 0;
            foreach (var amplifierId in AmplifierIds)
            {
                var amplifier = new Amplifier(_controllerSoftware, _phaseSettings[amplifierId], lastOutput);
                lastOutput = amplifier.GetOutput();
            }

            OutputToThrusters = lastOutput;
            return lastOutput;
        }

        public int GetSignalWithFeedbackLoops()
        {
            var amplifiers = new Dictionary<char, Amplifier>(5);

            var lastOutput = 0;
            for (var k = 0; k < AmplifierIds.Length; ++k)
            {
                var ampId = AmplifierIds[k];
                var amplifier = GetAmplifier(ampId);

                var generatedOutput = false;
                do
                {
                    generatedOutput = amplifier.Step();
                }
                while (!generatedOutput && !amplifier.IsHalted);

                if (generatedOutput)
                {
                    lastOutput = amplifier.Output.Last();
                }

                if (ampId == 'E' && !amplifier.IsHalted)
                {
                    k = -1; // start over
                }
            }

            OutputToThrusters = lastOutput;
            return lastOutput;

            Amplifier GetAmplifier(char id)
            {
                if (amplifiers.TryGetValue(id, out var amplifier))
                {
                    amplifier.Input.Enqueue(lastOutput);
                    return amplifier;
                }

                amplifiers[id] = amplifier = new Amplifier(_controllerSoftware, _phaseSettings[id], lastOutput);
                return amplifier;
            }
        }
    }

    public class Amplifier
    {
        private readonly IntcodeComputer _computer;

        public bool IsHalted => _computer.IsHalted;

        public Queue<int> Input => _computer.InputValues;

        public IReadOnlyList<int> Output => _computer.OutputValues;

        public Amplifier(IList<int> program, int phaseSetting, int inputSignal)
        {
            _computer = new IntcodeComputer(program, phaseSetting, inputSignal);
        }

        public bool Step() => _computer.Step();

        public int GetOutput()
        {
            _computer.RunToHalt();
            return _computer.OutputValues.First();
        }
    }
}