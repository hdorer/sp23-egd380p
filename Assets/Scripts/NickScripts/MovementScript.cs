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
        
        Vector2 temp = new Vector2(horiz,vert);
        //Debug.Log(temp);
        if(cam.transform.eulerAngles.x != 90)
            temp = Quaternion.Euler(cam.transform.eulerAngles.x,cam.transform.eulerAngles.y,cam.transform.eulerAngles.z)*temp;
        //Debug.Log(temp);
        rb.velocity = new Vector3(temp.x*runSpeed,0,temp.y*runSpeed);
    }
}
