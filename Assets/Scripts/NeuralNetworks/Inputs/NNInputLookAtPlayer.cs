using System.Collections;
using System.Collections.Generic;
using NeuralNetworks;
using UnityEngine;
using Utility;

namespace NeuralNetworks.Inputs {
    public class NNInputLookAtPlayer : RealtimeNeuralNetInput {
        [SerializeField] private float NormalizationAngleDegrees = 45.0f;
        private readonly float[] values = new float[1];

        public override int InputCount {
            get { return 1; }
        }

        public override float[] GetInput() {
            float angle = Vector2.Angle(transform.forward.xz(),
                player.transform.position.xz() - transform.position.xz());

            values[0] = 1 - Mathf.Exp(- angle / NormalizationAngleDegrees);
            Debug.LogFormat("NNInputLookAtPlayer [value={0}]", values[0]);
            return values;
        }
    }
}
