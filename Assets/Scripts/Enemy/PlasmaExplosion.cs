using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaExplosion : DeathTimer {
    [SerializeField] private float damage = 20f;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            other.GetComponent<MovementScript>().takeDamage(damage);
        }
    }
}
