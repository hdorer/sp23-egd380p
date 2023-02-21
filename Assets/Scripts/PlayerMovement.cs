using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 1f;
    public GameObject bullet;
    void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.position += new Vector3(horizontal, vertical, 0).normalized * Time.deltaTime * speed;

        if(Input.GetMouseButtonDown(0)) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float rot = Mathf.Atan2(pos.y - transform.position.y, pos.x - transform.position.x);
            float angle = (180 / Mathf.PI) * rot;
            GameObject firedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            firedBullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
