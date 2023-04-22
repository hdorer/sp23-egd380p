using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/Shotgun")]
public class Shotgun : Blaster {
    [Header("Shotgun Properties")]
    [SerializeField] private float bulletsPerShot;
    [SerializeField] private float bulletSpread;

    protected override void fire() {
        if (Player.playerMovement.onRolling)
        {
            return;
        }
        float startingRotation = -((bulletsPerShot - 1) / 2 * bulletSpread);
        for(int i = 0; i < bulletsPerShot; i++) {
            float bulletRotationX = Player.BulletSpawnRotation.eulerAngles.x;
            float bulletRotationY = Player.BulletSpawnRotation.eulerAngles.y + startingRotation + bulletSpread * i;
            float bulletRotationZ = Player.BulletSpawnRotation.eulerAngles.z;

            Quaternion bulletRotation = Quaternion.Euler(bulletRotationX, bulletRotationY, bulletRotationZ);

            Bullet bullet = Instantiate(BulletPrefab, Player.BulletSpawnPosition, bulletRotation).GetComponent<Bullet>();
            bullet.setDamage(DamagePerBullet * Player.DamageModifier);
            Debug.Log("Per-bullet damage: " + (DamagePerBullet * Player.DamageModifier));
        }
    }
}
