using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    [SerializeField] private float damage = 10f;

    private void OnCollisionEnter(Collision collision) { // should this be a trigger?
        if(collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<MovementScript>().takeDamage(damage);
        }
    }
}
