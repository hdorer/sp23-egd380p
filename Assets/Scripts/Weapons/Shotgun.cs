using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/Shotgun")]
public class Shotgun : Blaster {
    [Header("Shotgun Properties")]
    [SerializeField] private float bulletsPerShot;
    [SerializeField] private float bulletSpread;

    public override void fire() {
        float startingRotation = -((bulletsPerShot - 1) / 2 * bulletSpread);
        for(int i = 0; i < bulletsPerShot; i++) {
            float bulletRotationX = Player.BulletSpawnPointRotation.eulerAngles.x;
            float bulletRotationY = Player.BulletSpawnPointRotation.eulerAngles.y + startingRotation + bulletSpread * i;
            float bulletRotationZ = Player.BulletSpawnPointRotation.eulerAngles.z;

            Quaternion bulletRotation = Quaternion.Euler(bulletRotationX, bulletRotationY, bulletRotationZ);

            Bullet bullet = Instantiate(BulletPrefab, Player.BulletSpawnPoint, bulletRotation).GetComponent<Bullet>();
            bullet.setDamage(DamagePerBullet);
        }
    }
}
