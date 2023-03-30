using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/Laser")]
public class Laser : Weapon {
    private float heat = 0f;
    private float maxHeat = 100f;

    [Header("Laser Properties")]
    [SerializeField] private float passiveCooldownRate = 10f;
    [SerializeField] private float ventingCooldownRate = 30f;
    private bool venting = false;

    [SerializeField] private float maxCharge = 20f;
    private float charge = 0f;
    [SerializeField] private float chargeRate = 1f;
    private bool charging = false;

    [SerializeField] private float maxDamage = 40f;
    [SerializeField] private float maxHeatPerShot = 25f;

    [SerializeField] private float maxRange = 15f;

    public override void start() {

    }

    public override void update() {
        if(charging) {
            chargeShot();
        } else {
            coolOff();
        }

        Player.updateUi("Heat", (int)heat, (int)maxHeat);
    }

    public override void equip() {
        Player.updateUi("Heat", (int)heat, (int)maxHeat);
    }

    public override void startFiring() {
        if(venting) {
            return;
        }

        charging = true;
    }

    public override void stopFiring() {
        fire();
    }

    public override IEnumerator reload() {
        venting = true;
        yield return null;
    }

    private void fire() {
        if(!charging) {
            return;
        }

        float damage = maxDamage * (charge / maxCharge);
        Debug.Log("Damage Dealt: " + damage);

        float heat = maxHeatPerShot * (charge / maxCharge);
        this.heat += heat;
        if(this.heat > maxHeat) {
            Player.StartCoroutine(reload());
        }

        damageEnemy(damage);

        charge = 0;
        charging = false;
    }

    private void damageEnemy(float damage) {
        RaycastHit hit;
        Physics.Raycast(Player.BulletSpawnPoint, Player.BulletSpawnPointForward, out hit, maxRange);
        if(hit.collider == null) {
            return;
        }

        Enemy enemy = hit.collider.GetComponent<Enemy>();
        if(enemy == null) {
            return;
        }

        enemy.takeDamage();
    }

    private void chargeShot() {
        charge += chargeRate * Time.deltaTime;
        Debug.Log(charge);
        
        if(charge >= maxCharge) {
            fire();
        }
    }

    private void coolOff() {
        if(venting) {
            heat -= ventingCooldownRate * Time.deltaTime;
        } else {
            heat -= passiveCooldownRate * Time.deltaTime;
        }

        if(heat <= 0f) {
            heat = 0f;
            venting = false;
        }
    }
}
