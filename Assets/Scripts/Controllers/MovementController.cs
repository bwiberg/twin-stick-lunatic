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
        [SerializeField] private KeyCode DashKeyCode = KeyCode.LeftShift;
        [SerializeField] private bool CanMoveWhileAirborne;
        
        private CharacterController controller;

        private Vector3 velocity;
        private float timeOfLastDash = float.NegativeInfinity;

        private void Start() {
            controller = GetComponent<CharacterController>();
        }

        private void Update() {
            velocity.y = controller.isGrounded ? 0 : velocity.y;
            Move();
            Jump();
        }

        private void FixedUpdate() {
            var gravity = GravityMultiplier * Physics.gravity;
            velocity += gravity * Time.fixedDeltaTime;
            
            controller.Move(velocity * Time.fixedDeltaTime);
        }

        private void Move() {
            if (!controller.isGrounded && !CanMoveWhileAirborne) {
                return;
            }
            
            var input = GetInput();

            if (Input.GetKeyDown(DashKeyCode)) {
                timeOfLastDash = Time.time;
            }

            var forwardDir = Camera.main.transform.forward.CopySetY(0).normalized;
            var rightDir = Camera.main.transform.right.CopySetY(0).normalized;

            float speed = MovementSpeed;
            float timeSinceLastDash = Time.time - timeOfLastDash;
            if (timeSinceLastDash < DashDurationSeconds) {
                float a = timeSinceLastDash / DashDurationSeconds;
                speed = MovementSpeed * (a + DashMultiplier * (1 - a));
            }

            velocity = (speed * (input.y * forwardDir + input.x * rightDir)).CopySetY(velocity.y);
        }

        private void Jump() {
            if (!controller.isGrounded) {
                return;
            }
            
            if (CrossPlatformInputManager.GetButtonDown("Jump")) {
                velocity.y = JumpVelocity;
            }
        }

        private static Vector2 GetInput() {
            return new Vector2 {
                x = CrossPlatformInputManager.GetAxis("Horizontal"),
                y = CrossPlatformInputManager.GetAxis("Vertical")
            };
        }
    }
}
