using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyController : CustomBehaviour {
    void FixedUpdate() {
        Move();
    }

    protected abstract void Move();
}
