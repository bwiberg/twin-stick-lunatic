namespace Genetics.Fitness {
    public class ContinuousFitnessEvaluator : FitnessEvaluator {
        public override float ComputeForCreature(Creature creature) {
            return creature.GetComponent<ContinuousFitnessBehaviour>().Fitness;
        }
    }
}
