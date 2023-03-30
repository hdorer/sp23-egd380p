using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/Blaster")]
public class Blaster : Weapon {
    [Header("Blaster Properties")]
    [SerializeField] private float fireRate;
    [SerializeField] private float damagePerBullet;
    [SerializeField] private int clipSize;
    [SerializeField] private float reloadTime;

    private int bulletsInClip;

    [Header("Bullet Prefab")]
    [SerializeField] private GameObject bulletPrefab;
    
    private bool firing = false;
    private bool fireCooldown = false;
    private bool reloading = false;

    public float DamagePerBullet { get => damagePerBullet; }
    protected GameObject BulletPrefab { get => bulletPrefab; }

    public override void start() {
        bulletsInClip = clipSize;
    }

    public override IEnumerator reload() {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        bulletsInClip = clipSize;
        Player.updateUi("Ammo", bulletsInClip, clipSize);
        reloading = false;
    }

    public override void equip() {
        Player.updateUi("Ammo", bulletsInClip, clipSize);
    }

    public override void startFiring() {
        firing = true;
    }

    public override void stopFiring() {
        firing = false;
    }

    public override void update() {
        if(!firing || fireCooldown || reloading) {
            return;
        }

        fire();
        bulletsInClip--;

        Player.updateUi("Ammo", bulletsInClip, clipSize);

        if(bulletsInClip <= 0) {
            Player.StartCoroutine(reload());
        } else {
            Player.StartCoroutine(doFireCooldown());
        }
    }

    protected virtual void fire() {
        Bullet bullet = Instantiate(bulletPrefab, Player.BulletSpawnPosition, Player.BulletSpawnRotation).GetComponent<Bullet>();
        bullet.setDamage(damagePerBullet);
    }

    private IEnumerator doFireCooldown() {
        fireCooldown = true;
        yield return new WaitForSeconds(fireRate / Player.FireRateModifier);
        fireCooldown = false;
    }
}
