using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Utility;

namespace NeuralNetworks {
    public enum ActivationFunc {
        Passthrough,
        Step,
        SmoothTanh
    }

    public class RealtimeNeuralNet : CustomBehaviour {
        [SerializeField] private RealtimeNeuralNetInput[] Inputs;
        [SerializeField] private RealtimeNeuralNetOutput[] Outputs;
        [SerializeField] private int[] HiddenLayers;
        [SerializeField] private bool Recurrent;
        [SerializeField] private ActivationFunc ActivationFunc;
        [SerializeField, Range(1, 60)] private float UpdatesPerSecond = 1; 
        [SerializeField] public float[] Weights;
        [SerializeField] private bool RandomizeWeights;
        
        public int RequiredWeights {
            get { return FullyConnectedNN.RequiredWeights(NeuronsPerLayer); }
        }

        private FullyConnectedNN net;
        private float[] inputs;
        private float[] outputs;
      
        private int[] _NeuronsPerLayer;

        private int[] NeuronsPerLayer {
            get {
                _NeuronsPerLayer = new int[2 + HiddenLayers.Length];

                // number of outputs
                _NeuronsPerLayer[_NeuronsPerLayer.Length - 1] = Outputs.Select(input => input.OutputCount).Sum();

                // number of inputs
                _NeuronsPerLayer[0] = Inputs.Select(input => input.InputCount).Sum() +
                                      (Recurrent ? _NeuronsPerLayer[_NeuronsPerLayer.Length - 1] : 0);

                for (int i = 1; i <= HiddenLayers.Length; ++i) {
                    _NeuronsPerLayer[i] = HiddenLayers[i - 1];
                }

                return _NeuronsPerLayer;
            }
        }

        private int NumInputs {
            get { return NeuronsPerLayer[0]; }
        }

        private int NumOutputs {
            get { return NeuronsPerLayer[NeuronsPerLayer.Length - 1]; }
        }

        private ActivationFunction AF {
            get {
                switch (ActivationFunc) {
                    case ActivationFunc.Passthrough:
                        return NeuralNetworks.AF.Passthrough;
                    case ActivationFunc.SmoothTanh:
                        return NeuralNetworks.AF.SmoothTanh;
                    case ActivationFunc.Step:
                    default:
                        return NeuralNetworks.AF.BinaryMinusOneOrOne;
                }
            }
        }

        private float TimeBetweenUpdates {
            get { return 1f / UpdatesPerSecond; }
        }

        private void OnEnable() {
            net = new FullyConnectedNN(NeuronsPerLayer, AF);
            inputs = new float[NumInputs];
            outputs = new float[NumOutputs].Fill(0.0f);

            if (RandomizeWeights) {
                for (int i = 0; i < Weights.Length; ++i) {
                    Weights[i] = Random.Range(-1.0f, 1.0f);
                }
            }
        }

        private IEnumerator Run() {
            while (enabled) {
                Process();
                yield return new WaitForSeconds(TimeBetweenUpdates);
            }
        }

        private void Start() {
            ValidateWeights();
            StartCoroutine(Run());
        }

        private void Process() {
            // build up input array from input nodes
            int index = 0;
            foreach (var inputNode in Inputs) {
                float[] input = inputNode.GetInput();
                Assert.AreEqual(inputNode.InputCount, input.Length);
                foreach (float t in input) {
                    inputs[index++] = t;
                }
            }

            if (Recurrent) {
                foreach (float recurrentInput in outputs) {
                    inputs[index++] = recurrentInput;
                }
            }

            outputs = net.Evaluate(inputs, Weights);

            // send output to all output nodes
            index = 0;
            foreach (var outputNode in Outputs) {
                outputNode.HandleOutput(outputs, index);
                index += outputNode.OutputCount;
            }
        }

        private void ValidateWeights() {
            Assert.AreEqual(net.NumWeights, Weights.Length);
        }
    }
}
