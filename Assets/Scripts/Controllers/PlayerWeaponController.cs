using System.Collections.Generic;
using UnityEngine;
using Utility;
using Weapons;

namespace Controllers {
    public class PlayerWeaponController : CustomBehaviour {
        #region SERIALIZED_FIELDS

        [SerializeField] private Weapon[] WeaponPrefabs;
        [SerializeField] private Transform WeaponPivot;
        [SerializeField] private Collider Ground;

        #endregion

        #region PRIVATE_FIELDS

        private Weapon[] weaponInventory;
        private int activeWeaponIndex = 0;

        private Weapon ActiveWeapon {
            get { return weaponInventory[activeWeaponIndex]; }
        }

        #endregion

        private void Start() {
            weaponInventory = new Weapon[WeaponPrefabs.Length];
            for (int i = 0; i < WeaponPrefabs.Length; i++) {
                weaponInventory[i] = Instantiate(WeaponPrefabs[i].gameObject, WeaponPivot).GetComponent<Weapon>();
                weaponInventory[i].gameObject.SetActive(false);
            }
            weaponInventory[0].gameObject.SetActive(true);
        }

        private void Update() {
            LookAtMouse();

            if (Input.GetButton("Fire1")) {
                ActiveWeapon.TryUse();
                return;
            }

            SwitchWeapon();
        }

        private void SwitchWeapon() {
            int weaponIndex;
            if (!HasPressedSwitchKey(out weaponIndex)) {
                return;
            }

            if (weaponIndex >= weaponInventory.Length) {
                return;
            }

            Debug.Assert(activeWeaponIndex >= 0);

            ActiveWeapon.gameObject.SetActive(false);
            activeWeaponIndex = weaponIndex;
            ActiveWeapon.gameObject.SetActive(true);
        }

        private bool HasPressedSwitchKey(out int weaponIndex) {
            weaponIndex = -1;
            foreach (var kvp in WeaponIndicesByKeycode) {
                if (Input.GetKeyDown(kvp.Key)) {
                    weaponIndex = kvp.Value;
                    return true;
                }
            }
            return false;
        }

        private void LookAtMouse() {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (!Ground.Raycast(ray, out hit, 1000)) return;

            transform.LookAt(hit.point.CopySetY(transform.position.y));
        }

        #region STATIC_MEMBERS

        private static readonly IDictionary<KeyCode, int> WeaponIndicesByKeycode = new Dictionary<KeyCode, int>();

        static PlayerWeaponController() {
            WeaponIndicesByKeycode.Add(KeyCode.Alpha1, 0);
            WeaponIndicesByKeycode.Add(KeyCode.Alpha2, 1);
            WeaponIndicesByKeycode.Add(KeyCode.Alpha3, 2);
            WeaponIndicesByKeycode.Add(KeyCode.Alpha4, 3);
            WeaponIndicesByKeycode.Add(KeyCode.Alpha5, 4);
            WeaponIndicesByKeycode.Add(KeyCode.Alpha6, 5);
            WeaponIndicesByKeycode.Add(KeyCode.Alpha7, 6);
            WeaponIndicesByKeycode.Add(KeyCode.Alpha8, 7);
            WeaponIndicesByKeycode.Add(KeyCode.Alpha9, 8);
        }

        #endregion
    }
}
