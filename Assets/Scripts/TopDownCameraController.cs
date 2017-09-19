using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCameraController : MonoBehaviour {
    private GameObject player;
    [SerializeField] private Vector3 RelativePosition;
    [SerializeField] private Vector3 RelativeLookat;
    private bool isFirstFixedUpdate = true;
    private Vector3 previousLookat = Vector3.zero;

    [SerializeField] private bool Instant;
    [SerializeField, Range(0, 1)] private float CatchupFactorPosition;
    [SerializeField, Range(0, 1)] private float CatchupFactorLookat;

    private Transform PlayerTransform {
        get {
            return GameObject.FindGameObjectWithTag("Player").transform; 
        }
    }

    void FixedUpdate() {
        Transform player = PlayerTransform;
        
        if (!player) {
            throw new NullReferenceException("Reference to Player GameObject is null.");
        }

        Vector3 TargetPosition = player.transform.position + RelativePosition;
        Vector3 TargetLookat = player.transform.position + RelativeLookat;
        Vector3 actualPosition, actualLookat;

        if (Instant || isFirstFixedUpdate) {
            actualPosition = TargetPosition;
            actualLookat = TargetLookat;
            isFirstFixedUpdate = false;
        }
        else {
            actualPosition = Vector3.Lerp(transform.position, TargetPosition, CatchupFactorPosition);
            actualLookat = Vector3.Lerp(previousLookat, TargetLookat, CatchupFactorLookat);
        }

        transform.position = actualPosition; 
        transform.LookAt(actualLookat);

        previousLookat = actualLookat;
    }
}
