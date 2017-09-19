using System;

namespace NeuralNetworks {
    public delegate float ActivationFunction(float value);

    public static class AF {
        public static float BinaryMinusOneOrOne(float input) {
            return input >= 0.0f ? 1.0f : -1.0f;
        }
    
        public static float SmoothTanh(float input) {
            return (float) Math.Tanh(input);
        }
    
        public static float Passthrough(float input) {
            return input;
        }
    }
}
