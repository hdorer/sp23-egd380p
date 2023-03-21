using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBall : MonoBehaviour
{
    public float lifetime = 20;
    public float speed = 8;

    public bool isEnemy = true;

    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

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
