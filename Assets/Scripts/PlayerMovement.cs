using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    
    public float moveSpeed = 15f;
    public float maxSpeed = 20f;
    public float sprintMulti = 2f;
    public float sprintSpeedMulti = 1.5f;

    public float groundDrag;

    public float jumpForce = 20f;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    //change playerHeight if playercharacter height is changed
    public float playerHeight = 1.4f;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Awake(){
    DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {

        /*  TODO make sound when hitting ground
        *   
        *
        */
        // ground check
        RaycastHit hit;
        grounded = Physics.Raycast(transform.position, -Vector3.up, out hit, playerHeight);

        // handle drag
        if (grounded){
            rb.drag = groundDrag;
        }
        else{
            rb.drag = 0.5f;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void FixedUpdate()
    {

        //TODO keep velocity when landing or sound when landing for player feedback

        //Move player, probably expensive
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if(Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) < maxSpeed && !Input.GetKey(sprintKey)){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Acceleration);
        }
        //Sprint
        if(Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) < maxSpeed * sprintMulti && Input.GetKey(sprintKey)){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * sprintSpeedMulti, ForceMode.Acceleration);
            //Debug.Log("SPRINTING");
        }
    }


    private void Jump()
    {
        // reset y velocity
        Debug.Log("JUMPING");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    public void recoilKnockback(float knockback) {
        rb.AddForce(transform.forward * -knockback, ForceMode.Impulse);
    }
}