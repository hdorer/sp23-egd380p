using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour {
    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform bulletSpawnPoint;

    private int bulletsInClip;

    private bool firing = false;
    private bool fireCooldown = false;
    private bool reloading = false;

    [SerializeField] private InputAction fireInput;

    [System.Serializable] public class UiUpdateEvent : UnityEvent<int, int> { }
    public UiUpdateEvent onUiUpdate;

    private void OnEnable() {
        fireInput.Enable();

        fireInput.performed += context => firing = true;
        fireInput.canceled += context => firing = false;
    }

    private void Start() {
        bulletsInClip = weapon.ClipSize;
    }

    private void Update() {
        if(firing) {
            fire();
        }
    }

    private void OnDisable() {
        fireInput.Disable();
    }

    private void fire() {
        if(fireCooldown || reloading) {
            return;
        }

        weapon.fire(bulletSpawnPoint);
        bulletsInClip--;
        onUiUpdate?.Invoke(bulletsInClip, weapon.ClipSize);

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
        onUiUpdate?.Invoke(bulletsInClip, weapon.ClipSize);
        reloading = false;
    }
}
