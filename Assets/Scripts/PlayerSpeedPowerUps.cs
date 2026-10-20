using UnityEngine;
 
// Gives the player a speed boost for a limited time.
public class PlayerSpeedPowerUps : MonoBehaviour
{
    [SerializeField] private float boostMultiplier = 2f; // 50% faster
 
    // Seconds of boost left. The HUD reads it; only this script changes it.
    public float SpeedTimeLeft { get; private set; }
 
    private CharacterMovement movement;
 
    private void Start()
    {
        movement = GetComponent<CharacterMovement>();
    }
 
    // Called by the pickup. Collecting another boost restarts the timer.
    public void ActivateSpeedBoost(float duration)
    {
        SpeedTimeLeft = duration;
        movement.SpeedMultiplier = boostMultiplier;
    }
 
    private void Update()
    {
        if (SpeedTimeLeft <= 0f) return; // no boost running
 
        SpeedTimeLeft -= Time.deltaTime;
 
        if (SpeedTimeLeft <= 0f)
        {
            movement.SpeedMultiplier = 1f; // back to normal speed
        }
    }
}
