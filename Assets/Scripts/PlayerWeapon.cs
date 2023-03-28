using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour {
    [SerializeField] private Weapon[] weapons;
    private int currentWeapon = 0;
    
    [SerializeField] private Transform bulletSpawnPoint;

    private float fireRateModifier = 1.0f;

    [SerializeField] private WeaponPickup weaponPickupPrefab;
    private WeaponPickup nearbyPickup = null;
    private float pickupThrowForce = 2.0f;

    [SerializeField] private InputAction fireInput;
    [SerializeField] private InputAction reloadInput;
    [SerializeField] private InputAction changeWeaponInput;

    [System.Serializable] public class UiUpdateEvent : UnityEvent<string, int, int> { }
    public UiUpdateEvent onUiUpdate;

    [System.Serializable] public class WeaponSwitchEvent : UnityEvent<Weapon[], int> { }
    public WeaponSwitchEvent onWeaponSwitch;

    public Vector3 BulletSpawnPoint { get => bulletSpawnPoint.position; }
    public Quaternion BulletSpawnPointRotation { get => bulletSpawnPoint.rotation; }
    public float FireRateModifier { get => fireRateModifier; }

    private void OnEnable() {
        fireInput.Enable();
        reloadInput.Enable();
        changeWeaponInput.Enable();

        fireInput.performed += context => weapons[currentWeapon].startFiring();
        fireInput.canceled += context => weapons[currentWeapon].stopFiring();

        reloadInput.performed += context => StartCoroutine(weapons[currentWeapon].reload());

        changeWeaponInput.performed += context => changeWeapon();
    }

    private void Start() {
        for(int i = 0; i < weapons.Length; i++) {
            weapons[i].setPlayer(this);
            weapons[i].start();
        }

        weapons[currentWeapon].equip();
        onWeaponSwitch?.Invoke(weapons, currentWeapon);
    }

    private void Update() {
        weapons[currentWeapon].update();
    }

    private void OnDisable() {
        fireInput.Disable();
        reloadInput.Disable();
        changeWeaponInput.Disable();
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

    public void updateUi(string resourceName, int resource, int maxResource) {
        onUiUpdate?.Invoke(resourceName, resource, maxResource);
    }

    private void changeWeapon() {
        if(nearbyPickup == null) {
            return;
        }

        if(nearbyPickup.Weapon == null) {
            return;
        }

        spawnOldWeaponPickup();
        weapons[currentWeapon] = nearbyPickup.Weapon;
        Destroy(nearbyPickup.gameObject);
    }

    private void spawnOldWeaponPickup() {
        WeaponPickup pickup = Instantiate(weaponPickupPrefab, bulletSpawnPoint.position, transform.rotation);
        pickup.Weapon = weapons[currentWeapon];
        pickup.GetComponent<Rigidbody>().AddForce(transform.forward * pickupThrowForce, ForceMode.Impulse);
    }
}
