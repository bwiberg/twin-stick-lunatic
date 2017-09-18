using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

namespace NeuralNetworks {
    public delegate float ActivationFunction(float value);

    public class FullyConnectedNN {
        public int[] NeuronsPerLayer;
        public ActivationFunction ActivationFunction;

        public int LayerCount {
            get { return NeuronsPerLayer.Length; }
        }

        public int HiddenLayerCount {
            get { return LayerCount - 2; }
        }

        public int NeuronCount {
            get {
                int sum = 0;
                foreach (int n in NeuronsPerLayer) {
                    sum += n;
                }
                return sum;
            }
        }

        public FullyConnectedNN(int[] neuronsPerLayer, ActivationFunction activationFunction = null) {
            if (neuronsPerLayer.Length < 2) {
                throw new Exception("neuronsPerLayer.Length must be at least two (input- and output layers).");
            }
            NeuronsPerLayer = neuronsPerLayer;
            ActivationFunction = activationFunction ?? AF.Default;
        }

        public float[] Evaluate(float[] inputs, float[] weights) {
            int layer = 1;
            int weightOffset = 0;
            float[] outputs = null;
            while (layer < LayerCount) {
                outputs = EvaluateLayer(inputs, layer, weights, ref weightOffset);
                
                layer++;
                inputs = outputs;
            }
            return outputs;
        }

        public float[] EvaluateLayer(float[] inputs, int layer, float[] weights, ref int weightOffset) {
            int numNeurons = NeuronsPerLayer[layer];
            int numInputs = NeuronsPerLayer[layer - 1];

            float[] outputs = new float[numNeurons];

            for (int i = 0; i < numNeurons; ++i) {
                outputs[i] = EvaluateNeuron(inputs, i, weights, weightOffset);
                weightOffset += numInputs;
            }

            return outputs;
        }

        public float EvaluateNeuron(float[] inputs, int neuron, float[] weights, int weightOffset) {
            float weightedInputSum = 0.0f;
            for (int i = 0; i < inputs.Length; ++i) {
                weightedInputSum += weights[weightOffset + i] * inputs[i];
            }

            return ActivationFunction(weightedInputSum);
        }

        public static class AF {
            public static float Default(float input) {
                return input >= 0.0f ? 1.0f : -1.0f;
            }
    
            public static float Smooth(float input) {
                return (float) Math.Tanh(input);
            }
    
            public static float Passthrough(float input) {
                return input;
            }
        }
    }
}
