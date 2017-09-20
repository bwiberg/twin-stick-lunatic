using System.Collections;
using System.Collections.Generic;
using NeuralNetworks;
using UnityEngine;

namespace NeuralNetworks {
    namespace Inputs {
        public class NNInputDistanceToPlayer : RealtimeNeuralNetInput {
            [SerializeField] private float NormalizationDistance = 50.0f;
            private readonly float[] values = new float[1];
            
            public override int InputCount {
                get { return 1; }
            }

            public override float[] GetInput() {
                values[0] = Mathf.Min(1.0f,
                    (player.transform.position - transform.position).magnitude / NormalizationDistance);
                return values;
            }
        }
    }
}
