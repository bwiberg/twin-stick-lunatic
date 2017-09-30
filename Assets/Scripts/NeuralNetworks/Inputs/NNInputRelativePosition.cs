using System.Collections;
using System.Collections.Generic;
using NeuralNetworks;
using UnityEngine;

namespace NeuralNetworks.Inputs {
    public class NNInputRelativePosition : RealtimeNeuralNetInput {
        [SerializeField] private Transform target;
        [SerializeField] private Transform subtrahend;

        private readonly float[] values = new float[3];

        public override int InputCount {
            get { return 3; }
        }

        public override float[] GetInput() {
            values[0] = target.position.x - subtrahend.position.x;
            values[1] = target.position.y - subtrahend.position.y;
            values[2] = target.position.z - subtrahend.position.z;
            return values;
        }
    }
}
