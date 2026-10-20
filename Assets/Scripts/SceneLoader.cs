using UnityEngine;
using UnityEngine.SceneManagement; // needed for SceneManager

// Loads scenes by name. Buttons call LoadScene() and QuitGame()
// from their On Click list.
public class SceneLoader : MonoBehaviour
{
    // Public so a Button can call it. The scene must be in the
    // Build Profiles scene list or Unity will refuse to load it.
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        // Closes a built game. Does nothing inside the Unity Editor.
        Application.Quit();
    }
}
