using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/Laser")]
public class Laser : Weapon {
    private float heat = 0f;
    private float maxHeat = 100f;
    
    private float passiveCooldownRate = 10f;
    private float ventingCooldownRate = 50f;
    private bool venting = false;

    [Header("Laser Properties")]
    [SerializeField] private float maxCharge;
    private float charge = 0f;
    private float chargeRate = 1f;
    private bool charging = false;

    [SerializeField] private float maxDamage;
    private float maxHeatPerShot = 20f;

    [SerializeField] private float maxRange = 15f;

    public override IEnumerator reload() {
        charging = true;
        yield return null;
    }

    public override void equip() {
        Player.updateUi("Heat", (int)heat, (int)maxHeat);
    }

    public override void startFiring() {
        charging = true;
    }

    public override void stopFiring() {
        fire();
    }

    public override void update() {
        if(charging) {
            chargeShot();
        } else {
            coolOff();
        }

        Player.updateUi("Heat", (int)heat, (int)maxHeat);
    }

    private void fire() {
        if(!charging) {
            return;
        }

        RaycastHit hit;
        Physics.Raycast(Player.BulletSpawnPoint, Player.BulletSpawnPointForward, out hit, maxRange);

        Enemy enemy = hit.collider.GetComponent<Enemy>();
        if(enemy == null) {
            return;
        }

        float damage = maxDamage * (charge / maxCharge);
        float heat = maxHeatPerShot * (charge / maxCharge);

        Debug.Log("Damage Taken: " + damage);
        enemy.takeDamage();

        charging = false;
    }

    private void chargeShot() {
        charge += chargeRate * Time.deltaTime;
        
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
        }
    }
}
