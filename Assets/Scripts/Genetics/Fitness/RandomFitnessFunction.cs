using UnityEngine;

namespace Genetics.Fitness {
    public sealed class RandomFitnessFunction : FitnessEvaluator {
        public override float ComputeForCreature(Creature creature) {
            return Random.value;
        }
    }
}
