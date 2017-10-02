using UnityEngine;

namespace Genetics.Fitness {
    public abstract class ContinuousFitnessBehaviour : CustomBehaviour {
        [SerializeField] private UpdateMethod UpdateMethod;

        private void FixedUpdate() {
            if (UpdateMethod == UpdateMethod.FixedUpdate) {
                AccumulateFitness();
            }
        }

        private void LateUpdate() {
            if (UpdateMethod == UpdateMethod.LateUpdate) {
                AccumulateFitness();
            }
        }

        private void Update() {
            if (UpdateMethod == UpdateMethod.Update) {
                AccumulateFitness();
            }
        }

        protected abstract void AccumulateFitness();

        public abstract float Fitness { get; }
    }
}
