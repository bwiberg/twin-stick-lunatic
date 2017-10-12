using System.Collections;
using UnityEngine;

namespace Weapons {
    public class Gun : Weapon {
        #region SERIALIZED_FIELDS

        [SerializeField] private Projectile ProjectilePrefab;
        [SerializeField] private Transform Muzzle;

        #endregion

        protected override IEnumerator Use() {
            var projectile = Instantiate(ProjectilePrefab.gameObject, Muzzle.position, Muzzle.rotation)
                .GetComponent<Projectile>();
            projectile.IgnoreObject(gameObject);

            yield break;
        }
    }
}
