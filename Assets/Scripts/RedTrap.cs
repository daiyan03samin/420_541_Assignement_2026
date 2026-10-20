using UnityEngine;

public class RedTrap : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage();
            }
        }
    }
}
