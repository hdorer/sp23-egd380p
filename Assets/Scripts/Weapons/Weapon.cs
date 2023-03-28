using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Blaster")]
public class Weapon : ScriptableObject {
    [Header("Weapon Properties")]
    [SerializeField] private float fireRate;
    public float FireRate { get => fireRate; }
    [SerializeField] private float damagePerBullet;
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

    public virtual void fire(Transform bulletSpawnPoint) {
        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<Bullet>();
        bullet.setDamage(damagePerBullet);
    }
}
