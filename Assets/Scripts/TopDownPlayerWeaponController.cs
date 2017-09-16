using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPlayerWeaponController : MonoBehaviour {
    [SerializeField] private Collider ground;

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (ground.Raycast(ray, out hit, 1000)) {
            transform.LookAt(hit.point);
        }
    }
}