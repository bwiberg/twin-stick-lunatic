using UnityEngine;
using Utility;

namespace Genetics {
    public class BoxSpawnStateCreator : SpawnStateCreator {
        [SerializeField] private Bounds Box;
        [SerializeField] private bool LookAtPlayer;

        public override Tuple<Vector3, Quaternion> GenerateSpawnPoint(int indexInPopulation) {
            var position = Box.RandomWithin();

            Quaternion quat;
            if (LookAtPlayer) {
                quat = Quaternion.LookRotation((player.transform.position - position).CopySetY(0).normalized, Vector3.up);
            }
            else {
                quat = Quaternion.AngleAxis(360 * Random.value, Vector3.up);
            }

            return Tuple.Create(position, quat);
        }
    }
}
