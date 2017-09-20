using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NeuralNetworks {
    public abstract class RealtimeNeuralNetOutput : CustomBehaviour {
        public abstract int OutputCount { get; }

        public abstract void HandleOutput(float[] values, int offset);
    }
}
