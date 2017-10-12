using System.Collections;
using UnityEngine;
using UnityStandardAssets.Effects;

namespace Weapons {
    public abstract class Weapon : CustomBehaviour {
        #region SERIALIZED_FIELDS

        [SerializeField, Range(0, 60)] protected float Cooldown;

        #endregion

        #region PRIVATE_FIELDS

        private float timeOfLastUsage = float.NegativeInfinity;
        private bool isUsing;

        protected float TimeSinceLastUsage {
            get { return Time.time - timeOfLastUsage; }
        }

        #endregion

        public bool TryUse() {
            if (isUsing || TimeSinceLastUsage < Cooldown) {
                return false;
            }

            isUsing = true;
            StartCoroutine(InternalUse());

            return true;
        }

        private IEnumerator InternalUse() {
            yield return StartCoroutine(Use());
            isUsing = false;
            timeOfLastUsage = Time.time;
        }

        protected abstract IEnumerator Use();
    }
}
