using UnityEngine;
using TMPro;

// Shows the score the player finished the level with.
public class EndScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScoreText;

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // No ScoreManager object exists in this scene, but we can
        // still read the static FinalScore through the class name
        finalScoreText.text = "Final Score: " + GameManager.FinalScore;
    }
}
