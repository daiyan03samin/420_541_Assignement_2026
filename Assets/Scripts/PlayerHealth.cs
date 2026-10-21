using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;

    public void TakeDamage()
    {
        if (health > 0)
        {
            health--;
            Debug.Log("Health: " + health);
        }

        if (health == 0)
        {
            // GameManager.Instance.ResetScore();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
