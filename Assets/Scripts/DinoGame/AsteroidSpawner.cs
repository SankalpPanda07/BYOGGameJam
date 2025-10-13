using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject asteroidPrefab;
    public GameObject warningPrefab;

    [Header("Timing Settings")]
    public float minSpawnDelay = 1f; // Time between waves
    public float maxSpawnDelay = 3f;
    public float warningDuration = 2f;
    public float asteroidLifetime = 3f;

    [Header("References")]
    public Camera mainCamera;

    [Header("Wave Settings")]
    public float waveDuration = 20f; // How long each wave lasts
    public int[] asteroidsPerWave = { 1, 2, 3 }; // Number of asteroids per wave

    private float elapsedTime = 0f;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        StartCoroutine(SpawnAsteroidsRoutine());
    }

    IEnumerator SpawnAsteroidsRoutine()
    {
        while (true)
        {
            elapsedTime += Random.Range(minSpawnDelay, maxSpawnDelay);

            // Determine current wave
            int waveIndex = Mathf.FloorToInt(elapsedTime / waveDuration);
            waveIndex = Mathf.Clamp(waveIndex, 0, asteroidsPerWave.Length - 1);
            int asteroidsToSpawn = asteroidsPerWave[waveIndex];

            // Spawn multiple asteroids for this wave
            for (int i = 0; i < asteroidsToSpawn; i++)
            {
                Vector2 spawnPos = GetRandomPositionInCamera();

                // Spawn warning
                GameObject warning = Instantiate(warningPrefab, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(warningDuration);

                // Spawn asteroid
                GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);
                asteroid.tag = "Asteroid";
                Destroy(asteroid, asteroidLifetime);

                if (warning != null)
                    Destroy(warning);
            }

            // Wait random time before next set
            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);
        }
    }

    Vector2 GetRandomPositionInCamera()
    {
        Vector3 camMin = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 camMax = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float x = Random.Range(camMin.x + 0.5f, camMax.x - 0.5f);
        float y = Random.Range(camMin.y + 0.5f, camMax.y - 0.5f);

        return new Vector2(x, y);
    }
}
