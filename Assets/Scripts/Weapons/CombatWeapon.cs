using System;
using System.Collections;
using UnityEngine;

namespace Weapons {
    public class CombatWeapon : Weapon {
        #region SERIALIZED_FIELDS

        [SerializeField, Range(0.0f, 10.0f)] private float Duration;

        #endregion
        
        protected override IEnumerator Use() {
            var startTime = Time.time;

            float t = 0.0f;
            do {
                // animate
                Animate(t);

                yield return null;
                t = (Time.time - startTime) / Duration;
            } while (t < 1);
        }

        private void Animate(float t) {
            throw new NotImplementedException();
        }
    }
}
