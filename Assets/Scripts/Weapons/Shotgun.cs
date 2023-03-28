using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/Shotgun")]
public class Shotgun : Blaster {
    [Header("Shotgun Properties")]
    [SerializeField] private float bulletsPerShot;
    [SerializeField] private float bulletSpread;

    public override void fire(Transform bulletSpawnPoint) {
        float startingRotation = -((bulletsPerShot - 1) / 2 * bulletSpread);
        for(int i = 0; i < bulletsPerShot; i++) {
            float bulletRotationX = bulletSpawnPoint.rotation.eulerAngles.x;
            float bulletRotationY = bulletSpawnPoint.rotation.eulerAngles.y + startingRotation + bulletSpread * i;
            float bulletRotationZ = bulletSpawnPoint.rotation.eulerAngles.z;

            Quaternion bulletRotation = Quaternion.Euler(bulletRotationX, bulletRotationY, bulletRotationZ);

            Bullet bullet = Instantiate(BulletPrefab, bulletSpawnPoint.position, bulletRotation).GetComponent<Bullet>();
            bullet.setDamage(DamagePerBullet);
        }
    }
}
