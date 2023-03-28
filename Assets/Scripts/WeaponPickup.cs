using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour {
    [SerializeField] Weapon weapon;
    public Weapon Weapon { get => weapon; set => weapon = value; }

    private void OnTriggerEnter(Collider other) {
        PlayerWeapon pWeapon = other.GetComponent<PlayerWeapon>();
        if(pWeapon == null) {
            return;
        }

        pWeapon.setNearbyPickup(this);
    }

    private void OnTriggerExit(Collider other) {
        PlayerWeapon pWeapon = other.GetComponent<PlayerWeapon>();
        if(pWeapon == null) {
            return;
        }

        pWeapon.setNearbyPickup(null);
    }
}
