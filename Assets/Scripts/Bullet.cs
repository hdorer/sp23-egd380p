using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] private float speed = 10;
    private float damage = 0;
    private bool damageSet = false;

    [SerializeField] private float lifetime = 15;

    private void Start() {
        Destroy(gameObject, lifetime);
    }

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
