using System.Collections;

namespace Genetics {
    public abstract class Creature : CustomBehaviour {
        protected GeneticAlgorithm ga {
            get { return GeneticAlgorithm.Instance; }
        }
        
        public abstract void InitFromDNA(DNA dna);

        public abstract IEnumerator Live();
    }
}
