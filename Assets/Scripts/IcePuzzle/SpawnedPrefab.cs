using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpawnedPrefab : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject prefabToRespawn;    // The prefab to spawn again
    public Transform spawnPoint;          // Where the new prefab should appear

    private bool playerHasExited = false; // Track if player has left the trigger

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Only destroy if the player had exited before
        if (playerHasExited)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Mark that the player has exited
        playerHasExited = true;
    }

    void OnDestroy()
    {
        // When this object is destroyed → spawn a new one
        if (prefabToRespawn != null)
        {
            Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
            Instantiate(prefabToRespawn, spawnPos, Quaternion.identity);
            Debug.Log("🔄 Spawned new prefab after destruction!");
        }
    }
}