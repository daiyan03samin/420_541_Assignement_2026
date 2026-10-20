using UnityEngine;

// A speed boost pickup. When the player touches it, it gives the boost,
// plays a sound and particles, hides, and comes back a few seconds later.
public class SpeedPowerUpPickup : MonoBehaviour
{
    [SerializeField] private float duration = 5f;    // boost length
    [SerializeField] private float respawnDelay = 30f; // time hidden
    [SerializeField] private GameObject visuals;      // child to hide
    [SerializeField] private AudioClip speedPickupSound;

    private void OnTriggerEnter(Collider other)
    {
        // Only the player has a PlayerPowerUps component
        PlayerSpeedPowerUps speedPowerUps = other.GetComponentInParent<PlayerSpeedPowerUps>();
        if (speedPowerUps == null) return;

        speedPowerUps.ActivateSpeedBoost(duration);

        // Sound and particles
        GetComponent<AudioSource>().PlayOneShot(speedPickupSound);

        // Disappear, and call Reappear() after respawnDelay seconds
        visuals.SetActive(false);
        GetComponent<Collider>().enabled = false;
        Invoke(nameof(Reappear), respawnDelay);
    }

    private void Reappear()
    {
        visuals.SetActive(true);
        GetComponent<Collider>().enabled = true;
    }
}
