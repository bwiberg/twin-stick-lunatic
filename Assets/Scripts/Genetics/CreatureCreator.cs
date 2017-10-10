using UnityEngine;
using Utility;

namespace Genetics {
    public class CreatureCreator : CustomSingletonBehaviour<CreatureCreator> {
        [SerializeField] private Creature CreaturePrefab;
        [SerializeField] private SpawnStateCreator SpawnStateCreator;

        public int NumGenesRequired {
            get { return CreaturePrefab.NumGenesRequired; }
        }

        public Creature Create(DNA dna, int indexInPopulation) {
            Vector3 position;
            Quaternion quat;
            SpawnStateCreator.GenerateSpawnPoint(indexInPopulation).Unpack(out position, out quat);
            var creature = Instantiate(
                CreaturePrefab.gameObject,
                position,
                quat,
                transform
            ).GetComponent<Creature>();
            creature.DNA = dna;
            return creature;
        }
    }
}
