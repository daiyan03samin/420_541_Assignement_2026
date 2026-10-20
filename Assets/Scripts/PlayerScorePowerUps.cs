using UnityEngine;

public class PlayerScorePowerUps : MonoBehaviour
{
    public float ScoreTimeLeft { get; private set; }

    public void ActivateScoreBoost(float duration)
    {
        ScoreTimeLeft = duration;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ScoreMultiplier = 2f;
        }
    }

    private void Update()
    {
        if (ScoreTimeLeft <= 0f) return;

        ScoreTimeLeft -= Time.deltaTime;

        if (ScoreTimeLeft <= 0f)
        {
            ScoreTimeLeft = 0f;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ScoreMultiplier = 1f;
            }
        }
    }
}