using UnityEngine;
using Utility;

namespace Genetics {
    public class CreatureCreator : CustomSingletonBehaviour<CreatureCreator> {
        [SerializeField] private Creature[] CreaturePrefabs;
        
        public Creature Create(DNA dna) {
            var creature = Instantiate(CreaturePrefabs.GetRandom().gameObject).GetComponent<Creature>();
            creature.DNA = dna;
            return creature;
        }
    }
}
