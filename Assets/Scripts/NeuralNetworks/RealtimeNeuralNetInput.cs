using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NeuralNetworks {
    public abstract class RealtimeNeuralNetInput : CustomBehaviour, IRealtimeNeuralNetInput {
        public abstract int InputCount { get; }

        public abstract float[] GetInput();
    }
}
