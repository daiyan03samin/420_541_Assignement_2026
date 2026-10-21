using UnityEngine;
using TMPro; // TMP_Text

// Shows the speed boost icon and seconds left while the boost is on.
public class JumpPowerUpHUD : MonoBehaviour
{
    [SerializeField] private PlayerJumpPowerUps jumpPowerUps;
    [SerializeField] private GameObject jumpIndicator; // icon + time
    [SerializeField] private TMP_Text jumpTimeText;
    [SerializeField] private GameObject jumpIcon;

    private void Update()
    {
        float timeLeft = jumpPowerUps.JumpTimeLeft;

        // Visible for the whole boost, hidden the rest of the time
        jumpIndicator.SetActive(timeLeft > 0f);

        // CeilToInt rounds 6.2 up to 7, so we never show 0s too early
        jumpTimeText.text = Mathf.CeilToInt(timeLeft) + "s";

        // Make the icon blink like warning blink during the last 3 seconds of the boost
        if (timeLeft > 0 && timeLeft <= 5f)
        {
            jumpIcon.SetActive(Time.time % 1f < 0.5f);
        }
        else
        {
            jumpIcon.SetActive(true);
        }
    }
}
