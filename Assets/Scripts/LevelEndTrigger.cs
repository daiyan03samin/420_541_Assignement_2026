using UnityEngine;
using UnityEngine.SceneManagement;

// Put on a trigger at the end of the level.
// When the player enters it, save the score and load the End scene.
public class LevelEndTrigger : MonoBehaviour
{
    [SerializeField] private string endSceneName = "EndScene";

    private void OnTriggerEnter(Collider other)
    {
        // Only the player can finish the level
        if (!other.CompareTag("Player")) return;

        // Save the score BEFORE loading: the GameManager is about
        // to be destroyed together with the rest of SampleScene
        GameManager.Instance.SaveFinalScore();

        SceneManager.LoadScene(endSceneName);
    }
}
