using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
public class Bullet : DeathTimer {
    [SerializeField] private float speed = 10;
    private float damage;
    protected float Damage { get => damage; }
    private bool damageSet = false;

    private void Update() {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    public void setDamage(float damage) {
        if(!damageSet) {
            this.damage = damage;
            damageSet = true;
        }
    }

    void OnCollisionEnter(Collision col) {
        dealDamage(col);

        if(col.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
        }
    }

    protected virtual void dealDamage(Collision col) {

    }
}
