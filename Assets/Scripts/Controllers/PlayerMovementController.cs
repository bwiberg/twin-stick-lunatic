using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Utility;

namespace Controllers {
    [RequireComponent(typeof(MovementController))]
    public class PlayerMovementController : CustomBehaviour {
        [SerializeField] private KeyCode DashKeyCode = KeyCode.LeftShift;

        private MovementController movement;
        
        private void Start() {
            movement = GetComponent<MovementController>();
        }
        
        private void Update() {
            var input = GetInput();
            var dash = Input.GetKeyDown(DashKeyCode);
            
            var forwardDir = Camera.main.transform.forward.CopySetY(0).normalized;
            var rightDir = Camera.main.transform.right.CopySetY(0).normalized;

            var direction = input.y * forwardDir + input.x * rightDir;
            
            movement.Move(direction, dash);
            
            if (CrossPlatformInputManager.GetButtonDown("Jump")) {
                movement.Jump();
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
