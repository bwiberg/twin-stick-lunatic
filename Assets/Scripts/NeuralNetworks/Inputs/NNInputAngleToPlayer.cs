using System.Collections;
using System.Collections.Generic;
using NeuralNetworks;
using UnityEngine;
using Utility;

namespace NeuralNetworks {
    namespace Inputs {
        public class NNInputAngleToPlayer : RealtimeNeuralNetInput {
            [SerializeField] private float NormalizationAngleDegrees = 45.0f;
            private readonly float[] values = new float[1];

            public override int InputCount {
                get { return 1; }
            }

            public override float[] GetInput() {
                float signedAngle = Vector2.SignedAngle(transform.forward.xz(),
                    player.transform.position.xz() - transform.position.xz());

                values[0] = Mathf.Clamp(signedAngle / NormalizationAngleDegrees, -1.0f, 1.0f);
                return values;
            }
        }
    }
}
