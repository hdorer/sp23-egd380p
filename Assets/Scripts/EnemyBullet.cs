using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet {
    [SerializeField] private float bulletDamage = 5;

    private void Start() {
        setDamage(bulletDamage);
    }

    protected override void dealDamage(Collision col) {
        if(col.gameObject.CompareTag("Player")) {
            col.gameObject.GetComponent<MovementScript>().takeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
