using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject {
    [Header("Weapon Properties")]
    [SerializeField] private float fireRate;
    public float FireRate { get => fireRate; }
    [SerializeField] private float damagePerBullet;
    [SerializeField] private float bulletsPerShot; // I'm gonna do this later
    [SerializeField] private float bulletSpread;
    public float DamagePerBullet { get => damagePerBullet; }
    [SerializeField] private int clipSize;
    public int ClipSize { get => clipSize; }
    [SerializeField] private float reloadTime;
    public float ReloadTime { get => reloadTime; }

    [Header("Bullet Prefab")]
    [SerializeField] private GameObject bulletPrefab;
    protected GameObject BulletPrefab { get => bulletPrefab; }

    [Header("UI Properties")]
    [SerializeField] string gunName;
    [SerializeField] string flavorText;

    public void fire(Transform bulletSpawnPoint) {
        float startingRotation = -((bulletsPerShot - 1) / 2 * bulletSpread);
        for(int i = 0; i < bulletsPerShot; i++) {
            float bulletRotationX = bulletSpawnPoint.rotation.eulerAngles.x;
            float bulletRotationY = bulletSpawnPoint.rotation.eulerAngles.y + startingRotation + bulletSpread * i;
            float bulletRotationZ = bulletSpawnPoint.rotation.eulerAngles.z;

            Quaternion bulletRotation = Quaternion.Euler(bulletRotationX, bulletRotationY, bulletRotationZ);
            
            Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation).GetComponent<Bullet>();
            bullet.setDamage(damagePerBullet);
        }
    }
}
