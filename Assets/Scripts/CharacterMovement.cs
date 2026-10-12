using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves the player with a Rigidbody:
// A/D or Left/Right arrows to ROTATE, W/S or Up/Down to move Forward/Backward, Shift to Run and Space to jump.
[RequireComponent(typeof(Rigidbody))] // Ensures that a Rigidbody component is attached to the GameObject
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;      // units per second
    [SerializeField] private float currentSpeed;        // sprint logic
    [SerializeField] private float turnSpeed = 150f;    // degrees per second
    [SerializeField] private float jumpForce = 5f;      // strength of the jump
    [SerializeField] private float maxHoldTime = 3f;    // after max hold time, it automatically do the jump 
    [SerializeField] private float holdTime;
    [SerializeField] private float minJumpForce;        // min jump force
    [SerializeField] private float maxJumpForce;        // max jump force
    [SerializeField] private float doubleJumpForce = 4f;// strength of double jump
    [SerializeField] private bool hasDoubleJumpPowerUp = false;

    [Header("Ground Check Settings")]
    [SerializeField] private float groundDistance = 0.5f; // ray length
    [SerializeField] private LayerMask groundMask;        // ground layers

    private Rigidbody rb;                     // the physics body we move
    private Vector3 moveDirection;            // worked out from input in Update()
    private float turnInput;                  // horizontal input for rotation
    private bool isGrounded;                  // true when standing on the ground
    private bool jumpRequested = false;       // set in Update(), used later
    private bool isCharging = false;          // Trriger when hold the button
    private bool doubleJumpRequested = false; // set in TryDoubleJump()
    private bool hasDoubleJumped = false;     // one per jump

    // Lets other scripts READ isGrounded, but not change it.
    // "=> isGrounded" means "when asked, return isGrounded".
    public bool IsGrounded => isGrounded;

    // The methods from the next steps go here, inside the class
    // Start is called before the first frame update
    private void Start()
    {
        // Find the Rigidbody on this GameObject so we can move it
        rb = GetComponent<Rigidbody>();

        // Freeze rotation so the player doesn't fall over when colliding
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    private void Update()
    {
        // 1. Ground Check
        isGrounded = Physics.Raycast(transform.position + transform.up * groundDistance / 2, -transform.up, groundDistance, groundMask);

        // Back on the ground: the double jump is available again
        if (isGrounded)
        {
            hasDoubleJumped = false;
        }

        // 2. Read Inputs
        turnInput = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right turn input
        float moveZ = Input.GetAxisRaw("Vertical");  // W/S or Up/Down forward/backward

        // Rotate the player transform in Update for responsive turning visuals
        transform.Rotate(0f, turnInput * turnSpeed * Time.deltaTime, 0f);

        // Calculate forward movement relative to current facing direction
        moveDirection = transform.forward * moveZ;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = moveSpeed * 2;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // 3. Handle Jump Input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isCharging = true;
            holdTime = 0;
        }

        if (Input.GetButton("Jump") && isCharging)
        {
            holdTime += Time.deltaTime;

            if (holdTime >= maxHoldTime)
            {
                jumpRequested = true;
                isCharging = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (isCharging)
            {
                jumpRequested = true;
                isCharging = false;
            }
        }

        if (Input.GetButtonDown("Jump") && !isGrounded)
        {
            TryDoubleJump();
        }
    }

    private void FixedUpdate()
    {
        movePlayer();

        if (jumpRequested)
        {
            float force;

            force = Mathf.Lerp(minJumpForce, maxJumpForce, holdTime / maxHoldTime);

            Jump(force);

            holdTime = 0;
            jumpRequested = false;
        }

        if (doubleJumpRequested)
        {
            Jump(doubleJumpForce);
            doubleJumpRequested = false;
        }
    }

    private void movePlayer()
    {
        // Calculate velocity based on current forward vector
        Vector3 targetVelocity = moveDirection * currentSpeed;

        // Apply movement while preserving vertical velocity (gravity/jumping)
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    private void Jump(float force)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.VelocityChange);
    }

    // Tries to jump a second time in mid-air.
    // Returns true if the double jump really happened.
    private bool TryDoubleJump()
    {
        // Not allowed on the ground, or if it was already used
        if (isGrounded || hasDoubleJumped || !hasDoubleJumpPowerUp) return false;

        doubleJumpRequested = true;
        hasDoubleJumped = true;
        return true;
    }
}
