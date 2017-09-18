using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomBehaviour : MonoBehaviour {
    protected GameObject player {
        get { return GameObject.FindGameObjectWithTag("Player"); }
    }
}
