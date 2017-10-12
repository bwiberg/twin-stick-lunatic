using System.Collections;
using System.Collections.Generic;
using NeuralNetworks;
using UnityEngine;
using Utility;

namespace NeuralNetworks.Inputs {
    public class ViewFrustum : RealtimeNeuralNetInput {
        private float[] values;

        private bool playerIsWithinFrustum;
        private int RaycastMask;
        [SerializeField] private bool UseRaycasts;

        private float RaycastMaxDistance {
            get { return transform.lossyScale.z; }
        }
        
        public override int InputCount {
            get { return UseRaycasts ? 2 : 1; }
        }

        private void Awake() {
            RaycastMask = LayerMask.GetMask("Ground", "Enemy", "Player");
            values = new float[UseRaycasts ? 2 : 1];
        }

        public override float[] GetInput() {
            values[0] = playerIsWithinFrustum ? 1.0f : 0.0f;
            
            if (UseRaycasts)
                values[1] = RaycastProximityValue();
            
            // Debug.Log("Player is within frustum: " + playerIsWithinFrustum);
            // Debug.LogFormat("{0}: {1}", gameObject.name, values.ToStringWithSeparator());
            
            return values;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
                playerIsWithinFrustum = true;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
                playerIsWithinFrustum = false;
            }
        }

        private float RaycastProximityValue() {
            var ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, RaycastMaxDistance, RaycastMask)) {
                return 0.0f;
            }

            float closeness = hit.distance / RaycastMaxDistance;
            Debug.Assert(closeness >= 0 && closeness <= 1);
            return 1 - closeness;
        }
    }
}
