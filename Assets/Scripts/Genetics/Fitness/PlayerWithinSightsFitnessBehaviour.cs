using UnityEngine;

namespace Genetics.Fitness {
    public class PlayerWithinSightsFitnessBehaviour : ContinuousFitnessBehaviour {
        [SerializeField] private float FieldOfViewDegrees;

        protected override void AccumulateFitness() {
            var dirToPlayer = (player.transform.position - transform.position).normalized;
            var angle = Vector3.Angle(dirToPlayer, transform.forward);
            if (angle <= FieldOfViewDegrees) {
                fitness += 1.0f;
            }
        }

        private float fitness;

        public override float Fitness {
            get { return fitness; }
        }
    }
}
