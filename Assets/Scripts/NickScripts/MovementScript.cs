using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : Character
{
    [SerializeField] private InputAction dodgeRoll;
    //Made public so items can increase invincibility time
    [SerializeField] private float damageInvin = 1.0f;
    [SerializeField] private float rollInvin = 0.5f;

    private Rigidbody rb;
    private float horiz;
    private float vert;
    private bool invincible = false;

    private float moveSpeedModifier = 1.0f;

    private void OnEnable()
    {
        dodgeRoll.Enable();

        dodgeRoll.performed += onRoll;
    }
    private void OnDisable()
    {
        dodgeRoll.performed -= onRoll;

        dodgeRoll.Disable();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //Change inputs as new input system will be used.
        GetInputs();    //Simple get inputs

        RotatePlayer(); //Rotates the player to the mouse position

        if(Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Curent Health: " + health);
        }
    }
    private void RotatePlayer()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
 
        if(ground.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
    private void GetInputs() //Where you would put player input checks
    {
        horiz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");
    }
    private void MovePlayer()   //Moves player based on y rotation, so away from camera is always W and so on so forth.
    {
        Vector3 movement = new Vector3(horiz, 0, vert);
        movement = Quaternion.Euler(0,Camera.main.transform.eulerAngles.y,0)*movement;

        float mag = Mathf.Clamp01(movement.magnitude) * (moveSpeed * moveSpeedModifier);
        movement.Normalize();

        rb.velocity = movement*mag;
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private IEnumerator Invincibility(float duration)
    {
        invincible = true;
        yield return new WaitForSeconds(duration);
        invincible = false;
        
        StopCoroutine("Invincibility");
    }
    private void onRoll(InputAction.CallbackContext context)
    {
        if(invincible)
        {
            return;
        }

        StartCoroutine(Invincibility(rollInvin));

        //Play Animation

        //Maybe, cooldown on roll?
    }
    private void OnTriggerEnter(Collider col)
    {
        if(invincible)
        {
            return;
        }
        
        if(col.CompareTag("Enemy Bullet"))
        {
            //This need to take the damage the bullet does rather than a flat rate
            health -= 25; 
            //Update ui health!
            Debug.Log("Damaged");

            StartCoroutine(Invincibility(damageInvin));
        }
    }

    public void setMoveSpeedModifier(float modifier) {
        moveSpeedModifier = modifier;
    }

    public void resetMoveSpeedModifier() {
        moveSpeedModifier = 1.0f;
    }
}
