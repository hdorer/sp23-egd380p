using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet {
    protected override void dealDamage(Collision col) {
        if(col.gameObject.CompareTag("Enemy")) {
            col.gameObject.GetComponent<Enemy>().takeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
