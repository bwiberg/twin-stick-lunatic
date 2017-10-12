using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomBehaviour : MonoBehaviour {
    protected static GameObject player {
        get { return GameObject.FindGameObjectWithTag("Player"); }
    }

    private Rigidbody _rb;

    public Rigidbody rb {
        get { return _rb != null ? _rb : _rb = GetComponent<Rigidbody>(); }
    }
}
