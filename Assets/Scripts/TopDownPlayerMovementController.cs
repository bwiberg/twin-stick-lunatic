using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class TopDownPlayerMovementController : MonoBehaviour {
    [Serializable]
    public class MovementSettings {
        public float TargetSpeed = 1.0f;
        public float Acceleration = 1.0f;
        public float JumpForce = 10.0f;
        public float Slowdown = .75f;

        public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f),
            new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
    }


    [Serializable]
    public class AdvancedSettings {
        public float groundCheckDistance = 0.01f
            ; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )

        public float stickToGroundHelperDistance = 0.5f; // stops the character
        public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
        public bool airControl; // can the user control the direction that is being moved in the air

        [Tooltip("set it to 0.1 or more if you get stuck in wall")] public float shellOffset
            ; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
    }

    public MovementSettings movementSettings = new MovementSettings();
    public AdvancedSettings advancedSettings = new AdvancedSettings();


    private Rigidbody rigidbody;
    private CapsuleCollider capsule;
    private float m_YRotation;
    private Vector3 m_GroundContactNormal;
    private bool doJump, previouslyGrounded, isJumping, isGrounded;


    public Vector3 Velocity {
        get { return rigidbody.velocity; }
    }

    public bool Grounded {
        get { return isGrounded; }
    }

    public bool Jumping {
        get { return isJumping; }
    }

    private void Start() {
        rigidbody = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
    }


    private void Update() {
        if (CrossPlatformInputManager.GetButtonDown("Jump") && !doJump) {
            doJump = true;
        }
    }


    private void FixedUpdate() {
        GroundCheck();
        Vector2 input = GetInput();

//        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) &&
//            (advancedSettings.airControl || isGrounded)) {
        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon)) {
            Vector3 FORWARD = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
            Vector3 RIGHT = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up);

            Vector3 desiredMove = FORWARD * input.y + RIGHT * input.x;
            desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

            desiredMove.x = desiredMove.x * movementSettings.Acceleration;
            desiredMove.z = desiredMove.z * movementSettings.Acceleration;
            desiredMove.y = desiredMove.y * movementSettings.Acceleration;

            Vector3 diff = rigidbody.velocity - desiredMove;
            diff.y = 0.0f;

            rigidbody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);

            float magnitude = Mathf.Sqrt(Mathf.Pow(rigidbody.velocity.x, 2) + Mathf.Pow(rigidbody.velocity.z, 2));
            if (magnitude > movementSettings.TargetSpeed) {
                float x = rigidbody.velocity.x * movementSettings.TargetSpeed / magnitude;
                float z = rigidbody.velocity.z * movementSettings.TargetSpeed / magnitude;
                rigidbody.velocity = new Vector3(x, rigidbody.velocity.y, z);
            }
        }
        else {
            // slow down
            float x = rigidbody.velocity.x * movementSettings.Slowdown;
            float z = rigidbody.velocity.z * movementSettings.Slowdown;
            rigidbody.velocity = new Vector3(x, rigidbody.velocity.y, z);
        }

        if (isGrounded) {
            rigidbody.drag = 5f;

            if (doJump) {
                rigidbody.drag = 0f;
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
                rigidbody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                isJumping = true;
            }

            if (!isJumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon &&
                rigidbody.velocity.magnitude < 1f) {
                rigidbody.Sleep();
            }
        }
        else {
            rigidbody.drag = 0f;
            if (previouslyGrounded && !isJumping) {
                StickToGroundHelper();
            }
        }
        doJump = false;
    }


    private float SlopeMultiplier() {
        float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
        return movementSettings.SlopeCurveModifier.Evaluate(angle);
    }


    private void StickToGroundHelper() {
        RaycastHit hitInfo;
        if (!Physics.SphereCast(transform.position, capsule.radius * (1.0f - advancedSettings.shellOffset),
            Vector3.down, out hitInfo,
            ((capsule.height / 2f) - capsule.radius) +
            advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore)) return;
        
        if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f) {
            rigidbody.velocity = Vector3.ProjectOnPlane(rigidbody.velocity, hitInfo.normal);
        }
    }


    private Vector2 GetInput() {
        Vector2 input = new Vector2 {
            x = CrossPlatformInputManager.GetAxis("Horizontal"),
            y = CrossPlatformInputManager.GetAxis("Vertical")
        };
        return input;
    }

    /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
    private void GroundCheck() {
        previouslyGrounded = isGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, capsule.radius * (1.0f - advancedSettings.shellOffset),
            Vector3.down, out hitInfo,
            ((capsule.height / 2f) - capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers,
            QueryTriggerInteraction.Ignore)) {
            isGrounded = true;
            m_GroundContactNormal = hitInfo.normal;
        }
        else {
            isGrounded = false;
            m_GroundContactNormal = Vector3.up;
        }
        if (!previouslyGrounded && isGrounded && isJumping) {
            isJumping = false;
        }
    }
}
