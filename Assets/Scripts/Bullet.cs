using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
public class Bullet : MonoBehaviour {
    [SerializeField] private float speed = 10;
    private float damage;
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
}
