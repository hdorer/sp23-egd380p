using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {
    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform bulletSpawnPoint;

    private int bulletsInClip;
    
    private bool fireCooldown = false;
    private bool reloading = false;

    private void Start() {
        bulletsInClip = weapon.ClipSize;
    }

    private void Update() {
        if(Input.GetMouseButton(0)) {
            fire();
        }
    }

    private void fire() {
        if(fireCooldown || reloading) {
            return;
        }

        weapon.fire(bulletSpawnPoint);
        bulletsInClip--;

        if(bulletsInClip <= 0) {
            StartCoroutine(reload());
        } else {
            StartCoroutine(doFireCooldown());
        }
    }

    private IEnumerator doFireCooldown() {
        fireCooldown = true;
        yield return new WaitForSeconds(weapon.FireRate);
        fireCooldown = false;
    }

    private IEnumerator reload() {
        reloading = true;
        yield return new WaitForSeconds(weapon.ReloadTime);
        bulletsInClip = weapon.ClipSize;
        reloading = false;
    }
}
