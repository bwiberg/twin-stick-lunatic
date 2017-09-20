using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NeuralNetworks {
    public abstract class RealtimeNeuralNetOutput : CustomBehaviour, IRealtimeNeuralNetOutput {
        public abstract int OutputCount { get; }

        public abstract void HandleOutput(float[] values, int offset);
    }
}
