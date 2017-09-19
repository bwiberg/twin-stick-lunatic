using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NeuralNetworks;
using UnityEngine.Assertions;

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
        [SerializeField] private ActivationFunc ActivationFunc;
        [SerializeField] private UpdateFrequency UpdateFrequency;
        [SerializeField] public float[] Weights;

        public int RequiredWeights {
            get { return FullyConnectedNN.RequiredWeights(NeuronsPerLayer); }
        }

        private FullyConnectedNN net;
        private float[] inputs;

        private int[] NeuronsPerLayer {
            get {
                int[] neuronsPerLayer = new int[2 + HiddenLayers.Length];
                neuronsPerLayer[0] = Inputs.Length;
                for (int i = 1; i <= HiddenLayers.Length; ++i) {
                    neuronsPerLayer[i] = HiddenLayers[i - 1];
                }
                neuronsPerLayer[neuronsPerLayer.Length - 1] = Outputs.Length;
                return neuronsPerLayer;
            }
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

        private void Awake() {
            net = new FullyConnectedNN(NeuronsPerLayer, AF);
            inputs = new float[Inputs.Length];
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
            for (int i = 0; i < Inputs.Length; ++i) {
                inputs[i] = Inputs[i].GetInput();
            }
            float[] outputs = net.Evaluate(inputs, Weights);
            for (int i = 0; i < Outputs.Length; ++i) {
                Outputs[i].HandleOutput(outputs[i]);
            }
        }

        private void ValidateWeights() {
            Assert.AreEqual(net.NumWeights, Weights.Length);
        }
    }
}
