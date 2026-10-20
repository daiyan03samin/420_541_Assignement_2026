using UnityEngine;

public class ScoreBoostPickup : MonoBehaviour
{
    [SerializeField] private float duration = 10f;
    [SerializeField] private float respawnDelay = 30f;
    [SerializeField] private GameObject visuals;
    [SerializeField] private AudioClip scorePickupSound;

    private void OnTriggerEnter(Collider other)
    {
        PlayerScorePowerUps scorePowerUps = other.GetComponentInParent<PlayerScorePowerUps>();
        if (scorePowerUps == null) return;

        scorePowerUps.ActivateScoreBoost(duration);

        GetComponent<AudioSource>().PlayOneShot(scorePickupSound);

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