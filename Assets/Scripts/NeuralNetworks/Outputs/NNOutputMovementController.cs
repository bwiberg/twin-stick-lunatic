using Controllers;
using UnityEngine;
using Utility;

namespace NeuralNetworks.Outputs {
    [RequireComponent(typeof(MovementController))]
    public class NNOutputMovementController : RealtimeNeuralNetOutput {
        public override int OutputCount {
            get { return 3; }
        }

        [SerializeField, Range(0, 360)] private float TurnSpeed;
        [SerializeField, Range(0, 1)] private float JumpThreshold;
        [SerializeField, Range(0, 1)] private float Responsiveness = 0.3f;

        private float targetForwardSpeed;
        private float targetAngularSpeed;

        private float currentForwardSpeed;
        private float currentAngularSpeed;

        private MovementController movement;
        
        private void Awake() {
            movement = GetComponent<MovementController>();
        }

        public override void HandleOutput(float[] values, int offset) {
            //Debug.LogFormat("{0}: {1}", gameObject.name, values.ToStringWithSeparator());
            
            var forward = values[offset + 0];
            // forward = Mathf.Abs(forward);
            var turn = values[offset + 1];
            var jump = values[offset + 2];

            if (jump >= JumpThreshold) {
                movement.Jump();
            }
            
            if (forward > 0.1)
                Debug.Log("Forward: " + forward);

            targetForwardSpeed = forward;
            targetAngularSpeed = TurnSpeed * turn;
        }

        private void FixedUpdate() {
            currentForwardSpeed += (targetForwardSpeed - currentForwardSpeed) * Responsiveness;
            currentAngularSpeed += (targetAngularSpeed - currentAngularSpeed) * Responsiveness;
            
            transform.RotateAround(transform.position, Vector3.up, Time.fixedDeltaTime * currentAngularSpeed);
            movement.Move(currentForwardSpeed * transform.forward, false);
        }
    }
}
