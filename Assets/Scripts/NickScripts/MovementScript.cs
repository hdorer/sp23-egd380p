using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    Rigidbody rb;
    public float runSpeed = 10.0f;

    float horiz;
    float vert;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    void GetInputs() //Where you would put player input checks
    {
        horiz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        
        Vector3 movement = new Vector3(horiz, 0, vert);
        movement = Quaternion.Euler(0,cam.transform.eulerAngles.y,0)*movement;

        float mag = Mathf.Clamp01(movement.magnitude)*runSpeed;
        movement.Normalize();

        rb.velocity = movement*mag;
    }
}
