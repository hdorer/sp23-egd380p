using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/Blaster")]
public class Blaster : Weapon {
    [Header("Weapon Properties")]
    [SerializeField] private float fireRate;
    [SerializeField] private float damagePerBullet;
    [SerializeField] private int clipSize;
    [SerializeField] private float reloadTime;

    [Header("Bullet Prefab")]
    [SerializeField] private GameObject bulletPrefab;

    public float FireRate { get => fireRate; }
    public float DamagePerBullet { get => damagePerBullet; }
    public int ClipSize { get => clipSize; }
    public float ReloadTime { get => reloadTime; }
    protected GameObject BulletPrefab { get => bulletPrefab; }

    public override void fire(Transform bulletSpawnPoint) {
        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<Bullet>();
        bullet.setDamage(damagePerBullet);
    }
}
