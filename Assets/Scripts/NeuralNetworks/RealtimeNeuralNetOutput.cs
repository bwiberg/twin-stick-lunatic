using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class RealtimeNeuralNetOutput : CustomBehaviour {
    public abstract void HandleOutput(float value);
}
