using System.Collections;
using UnityEngine;

namespace Weapons {
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : CustomBehaviour {
        #region SERIALIZED_VARIABLES 

        [SerializeField, Range(0.0f, 100.0f)] private float InitialSpeed;
        [SerializeField, Range(1.0f, 10.0f)] private float LifeLengthSeconds = 5.0f;

        #endregion

        #region PRIVATE_VARIABLES

        private Collider collider {
            get { return GetComponent<Collider>(); }
        }

        #endregion

        #region PUBLIC_METHODS

        public void IgnoreObject(GameObject toIgnore, bool recursiveIgnore = true) {
            var colliderToIgnore = toIgnore.GetComponent<Collider>();
            if (colliderToIgnore) {
                Physics.IgnoreCollision(collider, colliderToIgnore);
            }
            
            if (!recursiveIgnore) {
                return;
            }
            
            foreach (Transform childToIgnore in toIgnore.transform) {
                IgnoreObject(childToIgnore.gameObject);
            }
        }

        #endregion

        private void OnCollisionEnter(Collision other) {
            KillSelf();
        }

        private void Start() {
            FireSelf();
            StartCoroutine(KillAfterLifeLengthReached());
        }

        private void FireSelf() {
            rb.velocity = InitialSpeed * transform.forward;
        }
        
        private void KillSelf() {
            Destroy(gameObject);
        }

        private IEnumerator KillAfterLifeLengthReached() {
            yield return new WaitForSeconds(LifeLengthSeconds);
            KillSelf();
        }

        private void OnDestroy() {
            StopCoroutine("KillAfterLifeLengthReached");
        }
    }
}
