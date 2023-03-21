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

    private float fireRateModifier = 1.0f;

    [SerializeField] private WeaponPickup weaponPickupPrefab;
    private WeaponPickup nearbyPickup = null;
    private float pickupThrowForce = 2.0f;

    [SerializeField] private InputAction fireInput;
    [SerializeField] private InputAction reloadInput;
    [SerializeField] private InputAction changeWeaponInput;

    [System.Serializable] public class UiUpdateEvent : UnityEvent<int, int> { }
    public UiUpdateEvent onUiUpdate;

    private void OnEnable() {
        fireInput.Enable();
        reloadInput.Enable();
        changeWeaponInput.Enable();

        fireInput.performed += context => firing = true;
        fireInput.canceled += context => firing = false;

        reloadInput.performed += context => StartCoroutine(reload());

        changeWeaponInput.performed += context => changeWeapon();
    }

    private void Start() {
        bulletsInClip = weapon.ClipSize;

        onUiUpdate?.Invoke(bulletsInClip, weapon.ClipSize);

        Debug.Log("Fire rate modifier is " + fireRateModifier);
    }

    private void Update() {
        if(firing) {
            fire();
        }
    }

    private void OnDisable() {
        fireInput.Disable();
        reloadInput.Disable();
    }

    public void setFireRateModifier(float fireRateModifier) {
        this.fireRateModifier = fireRateModifier;
        Debug.Log("Fire rate modifier is " + fireRateModifier);
    }

    public void resetFireRateModifier() {
        fireRateModifier = 1.0f;
        Debug.Log("Fire rate modifier is " + fireRateModifier);
    }

    public void setNearbyPickup(WeaponPickup pickup) {
        nearbyPickup = pickup;
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
        float fireRate = weapon.FireRate / fireRateModifier;
        yield return new WaitForSeconds(fireRate);
        fireCooldown = false;
    }

    private IEnumerator reload() {
        reloading = true;
        yield return new WaitForSeconds(weapon.ReloadTime);
        bulletsInClip = weapon.ClipSize;
        onUiUpdate?.Invoke(bulletsInClip, weapon.ClipSize);
        reloading = false;
    }

    private void changeWeapon() {
        if(nearbyPickup == null) {
            return;
        }

        if(nearbyPickup.Weapon == null) {
            return;
        }

        spawnOldWeaponPickup();
        weapon = nearbyPickup.Weapon;
        Destroy(nearbyPickup.gameObject);
    }

    private void spawnOldWeaponPickup() {
        WeaponPickup pickup = Instantiate(weaponPickupPrefab, bulletSpawnPoint.position, transform.rotation);
        pickup.Weapon = weapon;
        pickup.GetComponent<Rigidbody>().AddForce(transform.forward * pickupThrowForce, ForceMode.Impulse);
    }
}
