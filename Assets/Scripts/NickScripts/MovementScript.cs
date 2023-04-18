using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : Character
{
    [Tooltip("Used To Set Dodge Roll Button")]
    [SerializeField] private InputAction dodgeRoll;
    //Made public so items can increase invincibility time
    [Tooltip("The Time Invincible after taking damage")]
    [SerializeField] private float damageInvin = 1.0f;
    [Tooltip("The Time Invincible after rolling")]
    [SerializeField] private float rollInvin = 0.5f;
    [Tooltip("A Modifier to change distance of roll")]
    [SerializeField] private float rollMod = 50.0f;

    private Rigidbody rb;
    private float horiz;
    private float vert;
    private bool invincible = false;
    private bool onCooldown = false;
    private bool onRolling = false;

    private float moveSpeedModifier = 1.0f;
    private float damageTakenModifier = 1.0f;

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
            Debug.Log("Curent Health: " + Health);
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
        if(onRolling)
        {
            return;
        }
        rb.velocity = movement*mag;
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private IEnumerator DodgeRoll(float duration)
    {
        Debug.Log("Roll");
        invincible = true;
        onCooldown = true;
        onRolling = true;
        //Burst character in direction of movement
        Vector3 dir = Vector3.zero;

        if(rb.velocity.magnitude == 0)
        {
            invincible = false;
            onRolling = false;
            onCooldown = false;
            StopCoroutine("DodgeRoll");
        }
        else
        {
            dir = new Vector3 (horiz, 0, vert);
        }

        dir.Normalize();
        dir *= rollMod;
        //rb.velocity = dir;
        Vector3 newPos = transform.position+dir;
        transform.position = Vector3.MoveTowards(transform.position, newPos, duration);
        
        yield return new WaitForSeconds(duration);
        invincible = false;
        onRolling = false;
        yield return new WaitForSeconds(1.0f);
        onCooldown = false;
        
        StopCoroutine("DodgeRoll");
    }
    private IEnumerator DamageInv(float duration)
    {
        invincible = true;
        yield return new WaitForSeconds(duration);
        invincible = false;
        
        StopCoroutine("DamageInv");
    }
    private void onRoll(InputAction.CallbackContext context)
    {
        if(invincible || onCooldown)
        {
            return;
        }

        StartCoroutine(DodgeRoll(rollInvin));

        //Play Animation In coroutine?

        //Maybe, cooldown on roll?
    }

    public override void takeDamage(float damage) {
        if(invincible) {
            return;
        }

        base.takeDamage(damage * damageTakenModifier);

        if(Health <= 0) {
            Debug.Log("Oh no!  I am dead");
            // die
        }

        // update health UI

        Debug.Log("Damaged " + damage * damageTakenModifier);

        StartCoroutine(DamageInv(damageInvin));
    }

    public void setMoveSpeedModifier(float modifier) {
        moveSpeedModifier = modifier;
    }

    public void resetMoveSpeedModifier() {
        moveSpeedModifier = 1.0f;
    }

    public void setDamageTakenModifier(float modifier) {
        damageTakenModifier = modifier;
    }

    public void resetDamageTakenModifier() {
        damageTakenModifier = 1.0f;
    }
}
