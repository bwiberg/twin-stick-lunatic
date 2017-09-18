using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidEnemyController : EnemyController {
    [SerializeField, Range(0, 90)] private float TurnSpeed = 1.0f;
    [SerializeField, Range(0, 50)] private float MovementSpeed = 1.0f;
    [SerializeField, Range(0, 90)] private float FieldOfViewDegrees = 45.0f;

    private float turnAngularSpeed;

    void Awake() {
        turnAngularSpeed = Random.value >= 0.5f ? TurnSpeed : -TurnSpeed;
    }

    protected override void Move() {
        Vector3 playerDir = player.transform.position - transform.position;
        float distance = playerDir.magnitude;
        playerDir /= distance;
        float angleDegrees = Vector3.Angle(transform.forward, playerDir);
        
        Turn(playerDir, angleDegrees);
        Walk(playerDir, distance, angleDegrees);
    }

    private void Turn(Vector3 playerDir, float angleDegrees) {
        float angleIncrement = 0.0f;
        if (Mathf.Abs(angleDegrees) < FieldOfViewDegrees) {
            transform.forward = Vector3.RotateTowards(transform.forward, playerDir,
                Mathf.Deg2Rad * TurnSpeed * Time.fixedDeltaTime, 1.0f);
        }
        else {
            angleIncrement = turnAngularSpeed * Time.fixedDeltaTime;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, angleIncrement, 0));
        }
    }

    private void Walk(Vector3 playerDir, float distance, float angleDegrees) {
        if (angleDegrees >= FieldOfViewDegrees) {
            return;
        }
        
        transform.position += transform.forward * MovementSpeed * Time.fixedDeltaTime;
    }
}
