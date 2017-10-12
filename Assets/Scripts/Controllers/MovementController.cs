using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Utility;

namespace Controllers {
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : CustomBehaviour {
        [SerializeField, Range(0.0f, 100.0f)] private float MovementSpeed = 1.0f;
        [SerializeField, Range(1.0f, 10.0f)] private float DashMultiplier = 1.5f;
        [SerializeField, Range(0.0f, 10.0f)] private float DashDurationSeconds = 1.0f;
        [SerializeField, Range(0.0f, 100.0f)] private float JumpVelocity = 1.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float GravityMultiplier = 1.0f;
        [SerializeField] private bool CanMoveWhileAirborne;
        
        private CharacterController controller;

        private Vector3 velocity;
        private bool didJump;
        private float timeOfLastDash = float.NegativeInfinity;

        private void Awake() {
            controller = GetComponent<CharacterController>();
        }

        private void Update() {
            if (!didJump) {
                velocity.y = controller.isGrounded ? 0 : velocity.y;
            }
            didJump = false;
        }

        private void FixedUpdate() {
            var gravity = GravityMultiplier * Physics.gravity;
            velocity += gravity * Time.fixedDeltaTime;
            
            controller.Move(velocity * Time.fixedDeltaTime);
        }

        public void Move(Vector3 direction, bool dash) {
            if (!controller.isGrounded && !CanMoveWhileAirborne) {
                return;
            }

            if (dash)
                timeOfLastDash = Time.time;
            
            float speed = MovementSpeed;
            float timeSinceLastDash = Time.time - timeOfLastDash;
            if (timeSinceLastDash < DashDurationSeconds) {
                float a = timeSinceLastDash / DashDurationSeconds;
                speed = MovementSpeed * (a + DashMultiplier * (1 - a));
            }

            velocity = (speed * direction).CopySetY(velocity.y);
        }

        public void Jump() {
            if (!controller.isGrounded) {
                return;
            }

            didJump = true;
            velocity.y = JumpVelocity;
        }
    }
}
