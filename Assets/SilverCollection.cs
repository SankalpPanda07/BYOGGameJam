using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SilverOre : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Try to find the PlayerOreCollector component
            PlayerOreCollector collector = other.GetComponent<PlayerOreCollector>();
            if (collector != null)
            {
                collector.CollectOre();
            }

            // Destroy the ore after collection
            Destroy(gameObject);
        }
    }
}
