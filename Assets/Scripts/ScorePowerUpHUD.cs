using UnityEngine;
using TMPro; // TMP_Text

// Shows the speed boost icon and seconds left while the boost is on.
public class ScorePowerUpHUD : MonoBehaviour
{
    [SerializeField] private PlayerScorePowerUps powerUps;
    [SerializeField] private GameObject scoreIndicator; // icon + time
    [SerializeField] private TMP_Text scoreTimeText;
    [SerializeField] private GameObject scoreIcon;

    private void Update()
    {
        float timeLeft = powerUps.ScoreTimeLeft;

        // Visible for the whole boost, hidden the rest of the time
        scoreIndicator.SetActive(timeLeft > 0f);

        // CeilToInt rounds 6.2 up to 7, so we never show 0s too early
        scoreTimeText.text = Mathf.CeilToInt(timeLeft) + "s";

        // Make the icon blink like warning blink during the last 3 seconds of the boost
        if (timeLeft > 0 && timeLeft <= 3f)
        {
            scoreIcon.SetActive(Time.time % 1f < 0.5f);
        }
        else
        {
            scoreIcon.SetActive(true);
        }
    }
}
