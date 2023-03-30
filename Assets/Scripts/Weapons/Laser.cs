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
    [SerializeField] private float minHeatPerShot = 3f;
    [SerializeField] private float maxHeatPerShot = 25f;

    [SerializeField] private float maxRange = 15f;

    private RaycastHit hit;

    private float lrChargingAlpha = 0.6f;

    private float shotStayTime = 0.25f;

    public override void start() {
        
    }

    public override void update() {
        if(charging) {
            Physics.Raycast(Player.BulletSpawnPosition, Player.BulletSpawnForward, out hit, maxRange);
            chargeShot();
            updateLineRenderer();
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

        Player.LineRenderer.enabled = true;
        Player.LineRenderer.startColor = new Color(1, 0, 0, lrChargingAlpha);
        Player.LineRenderer.endColor = new Color(1, 0, 0, lrChargingAlpha);
        Player.LineRenderer.startWidth = 0;
        Player.LineRenderer.endWidth = 0;
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

        Player.StartCoroutine(showShot());
    }

    private void damageEnemy(float damage) {
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

    private void updateLineRenderer() {
        Player.LineRenderer.SetPosition(0, Player.BulletSpawnPosition);
        
        if(hit.collider != null) {
            Player.LineRenderer.SetPosition(1, hit.point);
        } else {
            Player.LineRenderer.SetPosition(1, Player.BulletSpawnPosition + Player.BulletSpawnForward * maxRange);
        }

        Player.LineRenderer.startWidth = charge / maxCharge;
        Player.LineRenderer.endWidth = charge / maxCharge;
    }

    private IEnumerator showShot() {
        Player.LineRenderer.startColor = new Color(1, 0, 0, 1);
        Player.LineRenderer.endColor = new Color(1, 0, 0, 1);
        
        yield return new WaitForSeconds(shotStayTime);

        Player.LineRenderer.enabled = false;
    }
}
