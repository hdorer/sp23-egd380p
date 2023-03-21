using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public struct HeldWeapon {
    public Weapon weapon;
    [HideInInspector] public int bulletsInClip;
}

public class PlayerWeapon : MonoBehaviour {
    [SerializeField] private HeldWeapon[] weapons;
    private int currentWeapon = 0;
    
    [SerializeField] private Transform bulletSpawnPoint;

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

    [System.Serializable] public class UiUpdateEvent : UnityEvent<HeldWeapon> { }
    public UiUpdateEvent onUiUpdate;

    [System.Serializable] public class WeaponSwitchEvent : UnityEvent<HeldWeapon[], int> { }
    public WeaponSwitchEvent onWeaponSwitch;

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
        for(int i = 0; i < weapons.Length; i++) {
            weapons[i].bulletsInClip = weapons[i].weapon.ClipSize;
        }

        onUiUpdate?.Invoke(weapons[currentWeapon]);
        onWeaponSwitch?.Invoke(weapons, currentWeapon);

        Debug.Log("Fire rate modifier is " + fireRateModifier);
    }

    private void Update() {
        if(firing) {
            fire();
        }
    }

    private void OnDisable() {
        changeWeaponsInput.performed -= changeWeapons;

        fireInput.Disable();
        reloadInput.Disable();
        changeWeaponsInput.Disable();
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

        weapons[currentWeapon].weapon.fire(bulletSpawnPoint);
        weapons[currentWeapon].bulletsInClip--;
        
        onUiUpdate?.Invoke(weapons[currentWeapon]);

        if(weapons[currentWeapon].bulletsInClip <= 0) {
            StartCoroutine(reload());
        } else {
            StartCoroutine(doFireCooldown());
        }
    }

    private IEnumerator doFireCooldown() {
        fireCooldown = true;
        float fireRate = weapons[currentWeapon].weapon.FireRate / fireRateModifier;
        yield return new WaitForSeconds(fireRate);
        fireCooldown = false;
    }

    private IEnumerator reload() {
        reloading = true;
        yield return new WaitForSeconds(weapons[currentWeapon].weapon.ReloadTime);
        weapons[currentWeapon].bulletsInClip = weapons[currentWeapon].weapon.ClipSize;
        onUiUpdate?.Invoke(weapons[currentWeapon]);
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
