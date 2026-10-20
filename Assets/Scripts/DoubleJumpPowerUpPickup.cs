using UnityEngine;

public class JumpBoostPickup : MonoBehaviour
{
    [SerializeField] private float duration = 30f;
    [SerializeField] private float respawnDelay = 30f;
    [SerializeField] private GameObject visuals;
    [SerializeField] private AudioClip jumpPickupSound;

    private void OnTriggerEnter(Collider other)
    {
        PlayerJumpPowerUps jumpPowerUps = other.GetComponentInParent<PlayerJumpPowerUps>();
        if (jumpPowerUps == null) return;

        jumpPowerUps.ActivateDoubleJump(duration);

        GetComponent<AudioSource>().PlayOneShot(jumpPickupSound);

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