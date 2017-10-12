using UnityEngine;
using Weapons;

namespace NeuralNetworks.Outputs {
    public class NNOutputWeaponController : RealtimeNeuralNetOutput {
        public override int OutputCount {
            get { return 1; }
        }

        [SerializeField, Range(0, 1)] private float FireThreshold = 0.5f;
        [SerializeField] private Weapon EquippedWeapon;

        private bool wantsToFire;

        public override void HandleOutput(float[] values, int offset) {
            wantsToFire = values[offset] >= FireThreshold;
        }

        private void Update() {
            if (wantsToFire)
                EquippedWeapon.TryUse();   
        }
    }
}
