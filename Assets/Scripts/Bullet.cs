using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isAllyBullet;
    public float speed = 10;
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
