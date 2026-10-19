using UnityEngine;

public class DoubleJumpEffect : MonoBehaviour
{
    [SerializeField] private AudioClip doubleJumpSound;
    [SerializeField] private GameObject doubleJumpEffect;

    public void Play()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        
        if (audioSource != null && doubleJumpSound != null)
        {
            audioSource.PlayOneShot(doubleJumpSound);

        }

        if (doubleJumpEffect != null)
        {
            // Vector3 effectPosition = transform.position + Vector3.down * 0.5f;
            Instantiate(doubleJumpEffect, transform.position, Quaternion.identity);
        }
    }
}