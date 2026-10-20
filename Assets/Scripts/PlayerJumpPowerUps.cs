using UnityEngine;

public class PlayerJumpPowerUps : MonoBehaviour
{
    public float JumpTimeLeft { get; private set; }

    private CharacterMovement movement;

    private void Start()
    {
        movement = GetComponent<CharacterMovement>();
    }

    public void ActivateDoubleJump(float duration)
    {
        if (movement == null) return;

        JumpTimeLeft = duration;
        movement.HasDoubleJumpPowerUp = true;
    }

    private void Update()
    {
        if (JumpTimeLeft <= 0f) return;

        JumpTimeLeft -= Time.deltaTime;

        if (JumpTimeLeft <= 0f)
        {
            JumpTimeLeft = 0f;
            movement.HasDoubleJumpPowerUp = false;
        }
    }
}