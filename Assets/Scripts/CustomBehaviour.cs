using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomBehaviour : MonoBehaviour {
    protected GameObject player {
        get { return GameObject.FindGameObjectWithTag("Player"); }
    }

    private Rigidbody _rb;

    protected Rigidbody rb {
        get { return _rb != null ? _rb : _rb = GetComponent<Rigidbody>(); }
    }
}
