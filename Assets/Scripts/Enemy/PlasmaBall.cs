using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBall : DeathTimer
{
    public float speed = 8;

    public bool isEnemy = true;

    public GameObject explosion;

    public void SetVelocity(Vector3 velocity)
    {
        GetComponent<Rigidbody>().velocity = velocity * 1.5f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
