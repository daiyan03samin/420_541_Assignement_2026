using UnityEngine;

// A coin that adds points to the score when the player touches it.
// Needs a collider with Is Trigger ticked on the same GameObject.
public class ScorePickup : MonoBehaviour
{
    // How many points this coin is worth (change it in the Inspector)
    [SerializeField] private int points = 50;
    [SerializeField] private AudioClip scorePickupSound;

    // Called automatically when something enters this trigger
    private void OnTriggerEnter(Collider other)
    {
        // Only react to the player, ignore everything else
        if (other.CompareTag("Player"))
        {
            // Add our points through the ScoreManager singleton
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(points);
            }

            if (scorePickupSound != null)
            {
                AudioSource.PlayClipAtPoint(scorePickupSound, transform.position, 3f);
            }

            // Remove the coin from the scene: it's collected once and gone
            Destroy(gameObject);
        }
    }
}
