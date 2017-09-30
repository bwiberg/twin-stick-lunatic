using UnityEngine;
using Utility;

namespace NeuralNetworks.Outputs {
    public class NNOutputMove : RealtimeNeuralNetOutput {
        [SerializeField] private float MaxSpeed = 1.0f;
        
        public override int OutputCount {
            get { return 1; }
        }

        public override void HandleOutput(float[] values, int offset) {
            float value = Mathf.Abs(values[offset]);
            
            Vector3 localVelocity = transform.worldToLocalMatrix * rb.velocity;
            rb.velocity = transform.localToWorldMatrix * localVelocity.CopySetZ(value * MaxSpeed);
            // rb.AddRelativeForce(0.0f, 0.0f, values[offset] * MaxSpeed, ForceMode.Impulse);
        }
    }
}
