using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NeuralNetworks;
using OSCsharp.Utils;
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
                neuronsPerLayer[0] = Inputs.Select(input => input.InputCount).Sum();

                for (int i = 1; i <= HiddenLayers.Length; ++i) {
                    neuronsPerLayer[i] = HiddenLayers[i - 1];
                }
                
                neuronsPerLayer[neuronsPerLayer.Length - 1] = Outputs.Select(input => input.OutputCount).Sum();
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
            if (!isActiveAndEnabled) return;
            
            net = new FullyConnectedNN(NeuronsPerLayer, AF);
            inputs = new float[NeuronsPerLayer[0]];
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
            // build up input array from input nodes
            int index = 0;
            foreach (RealtimeNeuralNetInput inputNode in Inputs) {
                float[] input = inputNode.GetInput();
                Assert.AreEqual(inputNode.InputCount, input.Length);
                foreach (float t in input) {
                    inputs[index++] = t;
                }
            }
            
            float[] outputs = net.Evaluate(inputs, Weights);
            
            // send output to all output nodes
            index = 0;
            foreach (RealtimeNeuralNetOutput outputNode in Outputs) {
                outputNode.HandleOutput(outputs, index);
                index += outputNode.OutputCount;
            }
        }

        private void ValidateWeights() {
            Assert.AreEqual(net.NumWeights, Weights.Length);
        }
    }
}
