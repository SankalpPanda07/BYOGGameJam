using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StepBlock : MonoBehaviour
{
    public GameObject prefabToSpawn;   // prefab to spawn on first step
    public AudioClip stepSound;        // sound to play on trigger
    public float volume = 1f;          // volume of the sound

    private AudioSource audioSource;

    void Awake()
    {
        // Add an AudioSource if not already present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Play sound
        if (stepSound != null)
        {
            audioSource.PlayOneShot(stepSound, volume);
        }

        // Spawn the prefab
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }

        // Destroy this block
        Destroy(gameObject);
    }
}
