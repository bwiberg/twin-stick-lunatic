﻿using System.Collections;
using System.Collections.Generic;
using NeuralNetworks;
using UnityEngine;

namespace NeuralNetworks.Inputs {
    public class NNInputViewFrustumProximity : RealtimeNeuralNetInput {
        private readonly float[] values = new float[3];

        private bool playerIsWithinFrustum;

        public override int InputCount {
            get { return 1; }
        }

        public override float[] GetInput() {
            values[0] = playerIsWithinFrustum ? 1.0f : 0.0f;
            return values;
        }

        private void OnTriggerEnter(Collider other) {
            Debug.LogFormat("OnTriggerEnter, name={0}", other.name);

            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
                playerIsWithinFrustum = true;
            }
        }

        private void OnTriggerLeave(Collider other) {
            Debug.LogFormat("OnTriggerLeave, name={0}", other.name);

            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
                playerIsWithinFrustum = false;
            }
        }
    }
}
