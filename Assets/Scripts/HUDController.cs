using UnityEngine;
using TMPro; // TMP_Text
 
// Updates the HUD every frame from the ScoreManager and PlayerPowerUps.
public class HUDController : MonoBehaviour
{
    // [Header("Player")]
    // [SerializeField] private PlayerScorePowerUps scorePowerUps;
 
    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;

    [Header("Health")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private PlayerHealth playerHealth;
 
 
    private void Update()
    {
        UpdateScore();
        UpdateHealth();
    }
 
    private void UpdateScore()
    {
        // The Score property from Lab 4: anyone can read it
        scoreText.text = "Score: " + GameManager.Instance.Score;
    }

    private void UpdateHealth()
    {
        healthText.text = "Health: " + playerHealth.health;
    }
 
}
