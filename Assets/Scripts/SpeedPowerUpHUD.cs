using UnityEngine;
using TMPro; // TMP_Text

// Shows the speed boost icon and seconds left while the boost is on.
public class SpeedPowerUpHUD : MonoBehaviour
{
    [SerializeField] private PlayerSpeedPowerUps speedPowerUps;
    [SerializeField] private GameObject indicator; // icon + time
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject speedIcon;

    private void Update()
    {
        float timeLeft = speedPowerUps.SpeedTimeLeft;

        // Visible for the whole boost, hidden the rest of the time
        indicator.SetActive(timeLeft > 0f);

        // CeilToInt rounds 6.2 up to 7, so we never show 0s too early
        timeText.text = Mathf.CeilToInt(timeLeft) + "s";

        // Make the icon blink like warning blink during the last 3 seconds of the boost
        if (timeLeft > 0 && timeLeft <= 3f)
        {
            speedIcon.SetActive(Time.time % 1f < 0.5f);
        }
        else
        {
            speedIcon.SetActive(true);
        }
    }
}
