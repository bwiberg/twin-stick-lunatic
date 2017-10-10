using UnityEngine;
using Utility;

namespace Genetics {
    public abstract class SpawnStateCreator : CustomSingletonBehaviour<SpawnStateCreator> {
        public abstract Tuple<Vector3, Quaternion> GenerateSpawnPoint(int indexInPopulation);
    }
}
