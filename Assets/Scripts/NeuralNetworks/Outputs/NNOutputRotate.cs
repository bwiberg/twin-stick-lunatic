using UnityEngine;
using Utility;

namespace NeuralNetworks.Outputs {
    [RequireComponent(typeof(Rigidbody))]
    public class NNOutputRotate : RealtimeNeuralNetOutput {
        [SerializeField] private float MaxAngularVelocity = 1.0f;
        [SerializeField] private float ValueThreshold = 0.0025f;

        public override int OutputCount {
            get { return 1; }
        }

        public override void HandleOutput(float[] values, int offset) {
            float value = values[offset];
            if (Mathf.Abs(value) < ValueThreshold) {
                value = 0.0f;
            }
            rb.angularVelocity = rb.angularVelocity.CopySetY(value * MaxAngularVelocity);
        }
    }
}
