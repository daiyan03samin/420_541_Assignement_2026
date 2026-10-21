using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Moves the player with a Rigidbody:
// A/D or Left/Right arrows to ROTATE, W/S or Up/Down to move Forward/Backward, Shift to Run and Space to jump.
[RequireComponent(typeof(Rigidbody))] // Ensures that a Rigidbody component is attached to the GameObject
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;      // units per second
    [SerializeField] private float currentSpeed;        // sprint logic
    [SerializeField] private float turnSpeed = 150f;    // degrees per second
    // [SerializeField] private float jumpForce = 5f;      // strength of the jump
    [SerializeField] private float maxHoldTime = 3f;    // after max hold time, it automatically do the jump 
    [SerializeField] private float holdTime;
    [SerializeField] private float minJumpForce;        // min jump force
    [SerializeField] private float maxJumpForce;        // max jump force
    [SerializeField] private float doubleJumpForce = 4f;// strength of double jump

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
    private DoubleJumpEffect doubleJumpEffect;// Double Jump Effect object
    private Vector3 startPosition;

    // Lets other scripts READ isGrounded, but not change it.
    // "=> isGrounded" means "when asked, return isGrounded".
    public bool IsGrounded => isGrounded;

    // Multiplies the normal speed. 1 = normal, 1.5 = 50% faster.
    // Other scripts (like PlayerPowerUps) are allowed to change it.
    public float SpeedMultiplier { get; set; } = 1f;

    public bool HasDoubleJumpPowerUp { get; set; }


    // The methods from the next steps go here, inside the class
    // Start is called before the first frame update
    private void Start()
    {
        // Find the Rigidbody on this GameObject so we can move it
        rb = GetComponent<Rigidbody>();

        // Freeze rotation so the player doesn't fall over when colliding
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        doubleJumpEffect = GetComponent<DoubleJumpEffect>();

        startPosition = transform.position;
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
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right turn input
        float moveZ = Input.GetAxisRaw("Vertical");  // W/S or Up/Down forward/backward

        // // Rotate the player transform in Update for responsive turning visuals
        // transform.Rotate(0f, moveX * turnSpeed * Time.deltaTime, 0f);

        // // Calculate forward movement relative to current facing direction
        // moveDirection = transform.forward * moveZ;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        // Rotate the player transform in Update for responsive turning visuals
        // Calculate forward movement relative to current facing direction
        moveDirection = cameraForward * moveZ + cameraRight * moveX;

        if (moveDirection.magnitude > 0.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime);
        }

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
            float force = Mathf.Lerp(minJumpForce, maxJumpForce, holdTime / maxHoldTime);

            Jump(force);

            holdTime = 0;
            jumpRequested = false;
        }

        if (doubleJumpRequested)
        {
            Jump(doubleJumpForce);
            doubleJumpEffect?.Play();
            doubleJumpRequested = false;
        }
    }

    private void movePlayer()
    {
        // Calculate velocity based on current forward vector
        Vector3 targetVelocity = moveDirection * currentSpeed * SpeedMultiplier;

        // Apply movement while preserving vertical velocity (gravity/jumping)
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

        // Debug.Log($"Base speed: {currentSpeed}, multiplier: {SpeedMultiplier}, effective speed: {currentSpeed * SpeedMultiplier}");
    }

    private void Jump(float force)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.VelocityChange);
    }

    // Tries to jump a second time in mid-air.
    // Returns true if the double jump really happened.
    public bool TryDoubleJump()
    {
        // Not allowed on the ground, or if it was already used
        if (isGrounded || hasDoubleJumped || !HasDoubleJumpPowerUp) return false;

        doubleJumpRequested = true;
        hasDoubleJumped = true;
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathPlane"))
        {
            // GameManager.Instance.ResetScore();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
