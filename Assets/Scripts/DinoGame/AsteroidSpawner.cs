using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject asteroidPrefab;
    public GameObject warningPrefab;

    [Header("Timing Settings")]
    public float minSpawnDelay = 3f;
    public float maxSpawnDelay = 7f;
    public float warningDuration = 2f;
    public float asteroidLifetime = 3f;

    [Header("References")]
    public Camera mainCamera;

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
            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);

            Vector2 spawnPos = GetRandomPositionInCamera();

            // 1️⃣ Spawn warning marker
            GameObject warning = Instantiate(warningPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(warningDuration);


            //CameraShake2D.Instance.Shake(3f, 0.5f);
            // 2️⃣ Spawn asteroid on the same spot
            GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);

            // Optional: set tag for collision detection
            asteroid.tag = "Asteroid";

            // 3️⃣ Destroy asteroid after lifetime
            Destroy(asteroid, asteroidLifetime);

            // Cleanup warning if still exists
            if (warning != null)
                Destroy(warning);
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
