using UnityEngine;

// Sends the player's movement to the Animator every frame
// so the right animation plays at the right time.
public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;       // plays the animations
    private CharacterMovement movement; // our movement script (for IsGrounded)
    private Rigidbody rb;            // used to read how fast the player moves

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the components on this same GameObject
        animator = GetComponent<Animator>();
        movement = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        // Send the current speed to the blend tree (Idle/Walking/Running).
        // Unity 6 calls the Rigidbody's velocity linearVelocity.
        animator.SetFloat("CharacterSpeed", rb.linearVelocity.magnitude);

        // On the ground? (controls Falling and ends the flip)
        // Tell the Animator if we're on the ground (controls Falling)
        bool grounded = movement.IsGrounded;
        animator.SetBool("IsGrounded", grounded);

        if (grounded)
        {
            // NEW: throw away a double jump request that was never used
            animator.ResetTrigger("doDoubleJump");
        }
        else
        {
            // NEW: Double jump. Space again in mid-air.
            // The flip only plays if PlayerMovement really jumped.
            if (Input.GetButtonDown("Jump") && movement.TryDoubleJump())
            {
                animator.SetTrigger("doDoubleJump");
            }
        }
    }
}
