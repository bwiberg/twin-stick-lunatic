using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using Utility;

namespace NeuralNetworks {
    public enum ActivationFunc {
        Passthrough,
        Step,
        SmoothTanh
    }

    public enum UpdateFrequency {
        Update,
        FixedUpdate
    }

    public class RealtimeNeuralNet : CustomBehaviour {
        [SerializeField] private RealtimeNeuralNetInput[] Inputs;
        [SerializeField] private RealtimeNeuralNetOutput[] Outputs;
        [SerializeField] private int[] HiddenLayers;
        [SerializeField] private bool Recurrent;
        [SerializeField] private ActivationFunc ActivationFunc;
        [SerializeField] private UpdateFrequency UpdateFrequency;
        [SerializeField] public float[] Weights;
        
        // DEBUG stuff
        
        [SerializeField] private bool RandomizeWeights;
        [SerializeField] private bool ShowDebugGUI;
        [SerializeField] private Rect DebugRect;
        [SerializeField] private float DebugRowHeight = 10.0f;
        
        public int RequiredWeights {
            get { return FullyConnectedNN.RequiredWeights(NeuronsPerLayer); }
        }

        private FullyConnectedNN net;
        private float[] inputs;
        private float[] outputs;
        
        // DEBUG
        private readonly IList<string> debugInputRows = new List<string>(); 
        private readonly IList<string> debugOutputRows = new List<string>(); 

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

        private void OnEnable() {
            Debug.Log("RealtimeNeuralNet enabled.");
            net = new FullyConnectedNN(NeuronsPerLayer, AF);
            inputs = new float[NumInputs];
            outputs = new float[NumOutputs].Fill(0.0f);

            if (RandomizeWeights) {
                for (int i = 0; i < Weights.Length; ++i) {
                    Weights[i] = Random.Range(-1.0f, 1.0f);
                }
            }
            
            Debug.LogFormat("Weights: {0}", string.Join(", ", Weights.Select(value => value.ToString()).ToArray()));
        }

        private void Start() {
            ValidateWeights();
        }

        private void Update() {
            if (UpdateFrequency == UpdateFrequency.Update) {
                Process();
            }
        }

        private void FixedUpdate() {
            if (UpdateFrequency == UpdateFrequency.FixedUpdate) {
                Process();
            }
        }

        private void Process() {
            debugInputRows.Clear();
            StringBuilder row = new StringBuilder();
            
            // build up input array from input nodes
            int index = 0;
            foreach (var inputNode in Inputs) {
                float[] input = inputNode.GetInput();
                row.AppendFormat("{0}: [", inputNode.GetType().Name);
                Assert.AreEqual(inputNode.InputCount, input.Length);
                foreach (float t in input) {
                    inputs[index++] = t;
                    row.AppendFormat("{0}, ", t);
                }
                row.Append("]");
                debugInputRows.Add(row.ToString());
                row.Length = 0;
            }

            if (Recurrent) {
                row.AppendFormat("Recurrency: [{0}]", string.Join(", ", outputs.Select(p => p.ToString()).ToArray()));
                debugOutputRows.Add(row.ToString());
                row.Length = 0;
                foreach (float recurrentInput in outputs) {
                    inputs[index++] = recurrentInput;
                }
            }

            outputs = net.Evaluate(inputs, Weights);
            debugOutputRows.Clear();

            // send output to all output nodes
            index = 0;
            foreach (var outputNode in Outputs) {
                row.AppendFormat("{0}: [", outputNode.GetType().Name);
                for (int i = 0; i < outputNode.OutputCount; ++i) {
                    row.AppendFormat("{0}, ", outputs[index + i]);
                }
                row.Append("]");
                debugOutputRows.Add(row.ToString());
                row.Length = 0;
                outputNode.HandleOutput(outputs, index);
                index += outputNode.OutputCount;
            }
        }

        private void ValidateWeights() {
            Assert.AreEqual(net.NumWeights, Weights.Length);
        }

        private void OnGUI() {
            if (!ShowDebugGUI) {
                return;
            }
            
            Rect rect = new Rect(DebugRect);
            GUI.Label(rect, "Inputs:");
            rect.y += DebugRowHeight - 10;
            foreach (var row in debugInputRows) {
                GUI.Label(rect, row);
                rect.y += DebugRowHeight;
            }
            
            GUI.Label(rect, "Outputs:");
            rect.y += DebugRowHeight - 10;
            foreach (var row in debugOutputRows) {
                GUI.Label(rect, row);
                rect.y += DebugRowHeight;
            }
        }
    }
}
