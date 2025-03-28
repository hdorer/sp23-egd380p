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
    private float damageModifier = 1.0f;

    [SerializeField] private WeaponPickup weaponPickupPrefab;
    private WeaponPickup nearbyPickup = null;
    private float pickupThrowForce = 2.0f;

    [SerializeField] private InputAction fireInput;
    [SerializeField] private InputAction reloadInput;
    [SerializeField] private InputAction changeWeaponInput;
    [SerializeField] private InputAction pickUpWeaponInput;

    public MovementScript playerMovement;

    [System.Serializable] public class UiUpdateEvent : UnityEvent<string, int, int> { }
    public UiUpdateEvent onUiUpdate;

    [System.Serializable] public class WeaponSwitchEvent : UnityEvent<Weapon[], int> { }
    public WeaponSwitchEvent onWeaponSwitch;

    public Vector3 BulletSpawnPosition { get => bulletSpawnPoint.position; }
    public Quaternion BulletSpawnRotation { get => bulletSpawnPoint.rotation; }
    public Vector3 BulletSpawnForward { get => bulletSpawnPoint.forward; }
    public LineRenderer LineRenderer { get => bulletSpawnPoint.GetComponent<LineRenderer>(); }
    public float FireRateModifier { get => fireRateModifier; }
    public float DamageModifier { get => damageModifier; }

    public GameObject[] guns;

    private void OnEnable() {
        fireInput.Enable();
        reloadInput.Enable();
        changeWeaponInput.Enable();
        pickUpWeaponInput.Enable();

        fireInput.performed += context => weapons[currentWeapon].startFiring();
        fireInput.canceled += context => weapons[currentWeapon].stopFiring();

        reloadInput.performed += context => StartCoroutine(weapons[currentWeapon].reload());

        changeWeaponInput.performed += changeWeapon;

        pickUpWeaponInput.performed += context => pickUpWeapon();
    }

    private void Start() {
        bulletSpawnPoint.GetComponent<LineRenderer>().enabled = false;

        for(int i = 0; i < weapons.Length; i++) {
            weapons[i] = Instantiate(weapons[i]);
            weapons[i].setPlayer(this);
            weapons[i].start();
        }
        playerMovement = GetComponent<MovementScript>();

        weapons[currentWeapon].equip();
        onWeaponSwitch?.Invoke(weapons, currentWeapon);
    }

    private void Update() {
        weapons[currentWeapon].update();
    }

    private void OnDisable() {
        fireInput.Disable();
        reloadInput.Disable();
        pickUpWeaponInput.Disable();
    }

    public void setFireRateModifier(float fireRateModifier) {
        this.fireRateModifier = fireRateModifier;
        Debug.Log("Fire rate modifier is " + fireRateModifier);
    }

    public void resetFireRateModifier() {
        fireRateModifier = 1.0f;
        Debug.Log("Fire rate modifier is " + fireRateModifier);
    }

    public void setDamageModifier(float damageModifier) {
        this.damageModifier = damageModifier;
    }

    public void resetDamageModifier() {
        damageModifier = 1.0f;
    }

    public void setNearbyPickup(WeaponPickup pickup) {
        nearbyPickup = pickup;
        Debug.Log(nearbyPickup == null ? "null" : nearbyPickup.gameObject.name);
    }

    public void updateUi(string resourceName, int resource, int maxResource) {
        onUiUpdate?.Invoke(resourceName, resource, maxResource);
    }

    private void pickUpWeapon() {
        if(nearbyPickup == null) {
            return;
        }

        if(nearbyPickup.Weapon == null) {
            return;
        }

        spawnOldWeaponPickup();
        
        weapons[currentWeapon] = Instantiate(nearbyPickup.Weapon);
        weapons[currentWeapon].setPlayer(this);
        weapons[currentWeapon].start();
        weapons[currentWeapon].equip();
        onWeaponSwitch?.Invoke(weapons, currentWeapon);
        
        Destroy(nearbyPickup.gameObject);
    }

    private void spawnOldWeaponPickup() {
        WeaponPickup pickup = Instantiate(weaponPickupPrefab, bulletSpawnPoint.position, transform.rotation);
        pickup.Weapon = weapons[currentWeapon];
        pickup.GetComponent<Rigidbody>().AddForce(transform.forward * pickupThrowForce, ForceMode.Impulse);
    }

    private void changeWeapon(InputAction.CallbackContext context) {
        int delta = (int)Mathf.Sign(context.ReadValue<float>());

        currentWeapon += delta;

        if(currentWeapon >= weapons.Length) {
            currentWeapon = 0;
        }
        if(currentWeapon < 0) {
            currentWeapon = weapons.Length - 1;
        }

        weapons[currentWeapon].equip();
        onWeaponSwitch?.Invoke(weapons, currentWeapon);

        //cheating a bit
        foreach(GameObject go in guns)
        {
            if(go != null) {
                go.SetActive(false);
            }
        }

        guns[weapons[currentWeapon].gunIndex].SetActive(true);
    }
}
