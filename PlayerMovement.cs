using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables
    public float moveSpeed = 50f;
    [SerializeField] float jumpForce = 20f;
    
    // Keybinds
    [SerializeField] KeyCode jump = KeyCode.Space;
    [SerializeField] KeyCode sprint = KeyCode.LeftShift;

    // Drag
    float groundDrag = 6f;
    float airDrag = 2f;

    // Multiplier
    [SerializeField] float airMult = 0.4f;


    public Rigidbody rb;

    public float horiMov;
    public float vertiMov;

    float playerHeight = 2f;

    [Header("Ground stuff")]
    [SerializeField] Transform groundDetection;
    [SerializeField] LayerMask ground; 
    bool isGrounded;
    float groundDistance = 0.4f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f; 


    // Orientation
    [SerializeField] Transform orientation;

    // Slope Handling
    RaycastHit slopeHit;

    public Vector3 direction;
    Vector3 slopeDirection; 

    
    // Default methods


    public void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    public void Update ()
    {
        MovementInput();
        Drag();
        contronSpeed();

        isGrounded = Physics.CheckSphere(groundDetection.position, groundDistance, ground);

        if (Input.GetKeyDown(jump) && isGrounded)
        {
            Jump();
        }

        slopeDirection = Vector3.ProjectOnPlane(direction, slopeHit.normal);
    }

    public void FixedUpdate ()
    {
        MovePlayer();
    }

    // Functions

    public void MovementInput()
    {
        horiMov = Input.GetAxisRaw("Horizontal");
        vertiMov = Input.GetAxisRaw("Vertical");

        direction = orientation.forward * vertiMov + orientation.right * horiMov;
    }

    public void MovePlayer ()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(direction.normalized * moveSpeed, ForceMode.Acceleration);

        } else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeDirection.normalized * moveSpeed, ForceMode.Acceleration);

        } else if (!isGrounded)
        {
            rb.AddForce(direction.normalized * moveSpeed * airMult, ForceMode.Acceleration);
        }
    }

    public void Drag ()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }else
        {
            rb.drag = airDrag;
        }
    }
    private bool OnSlope ()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }else
            {
                return false;
            }
        }
        return false; 
    }

    public void Jump ()
    {
        rb.velocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void contronSpeed ()
    {
        if (Input.GetKey(sprint) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }else
        {

            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }
}
