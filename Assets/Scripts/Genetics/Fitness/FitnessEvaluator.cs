namespace Genetics {
    public abstract class FitnessEvaluator : CustomBehaviour {
        public abstract float ComputeForCreature(Creature creature);
    }
}
