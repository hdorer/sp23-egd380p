using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    Rigidbody rb;
    private float runSpeed = 10.0f;
    float horiz;
    float vert;
    public Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        GetInputs();    //Simple get inputs

        RotatePlayer(); //Rotates the player to the mouse position
    }

    //Can use same code for shooting weapons
    void RotatePlayer()
    {
        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
 
        if(ground.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    void GetInputs() //Where you would put player input checks
    {
        horiz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");
    }

    void MovePlayer()   //Moves player based on y rotation, so away from camera is always W and so on so forth.
    {
        Vector3 movement = new Vector3(horiz, 0, vert);
        movement = Quaternion.Euler(0,cam.transform.eulerAngles.y,0)*movement;

        float mag = Mathf.Clamp01(movement.magnitude)*runSpeed;
        movement.Normalize();

        rb.velocity = movement*mag;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
}
