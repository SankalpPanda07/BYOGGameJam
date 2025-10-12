using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject prefabToSpawn;
    public Vector2 ySpawnRange = new Vector2(-3f, 3f);
    public Vector2 fadeDurationRange = new Vector2(3f, 8f);
    public Vector2 spawnCountRange = new Vector2(1, 3);

    [Header("Movement Settings")]
    [Tooltip("Horizontal movement speed range (random per object).")]
    public Vector2 moveSpeedRange = new Vector2(3f, 6f);

    [Tooltip("Wave amplitude (vertical movement range).")]
    public float waveAmplitude = 0.5f;

    [Tooltip("Wave frequency (how fast it oscillates).")]
    public float waveFrequency = 2f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            int spawnCount = Random.Range((int)spawnCountRange.x, (int)spawnCountRange.y + 1);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnObject();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void SpawnObject()
    {
        if (prefabToSpawn == null) return;

        Vector3 spawnPos = new Vector3(transform.position.x, Random.Range(ySpawnRange.x, ySpawnRange.y), 0f);
        GameObject obj = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        StartCoroutine(MoveAndFade(obj));
    }

    private IEnumerator MoveAndFade(GameObject obj)
    {
        float fadeTime = Random.Range(fadeDurationRange.x, fadeDurationRange.y);
        float moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
        float elapsed = 0f;

        float startY = obj.transform.position.y;
        float waveOffset = Random.Range(0f, Mathf.PI * 2f); // So each object has a unique phase

        var img = obj.GetComponent<UnityEngine.UI.Image>();
        var sprite = obj.GetComponent<SpriteRenderer>();

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;

            // Horizontal movement
            obj.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            // Wave-like vertical offset
            float waveY = Mathf.Sin((elapsed + waveOffset) * waveFrequency) * waveAmplitude;
            Vector3 pos = obj.transform.position;
            pos.y = startY + waveY;
            obj.transform.position = pos;

            // Fading effect
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);

            if (img != null)
            {
                Color c = img.color;
                c.a = alpha;
                img.color = c;
            }
            else if (sprite != null)
            {
                Color c = sprite.color;
                c.a = alpha;
                sprite.color = c;
            }

            yield return null;
        }

        Destroy(obj);
    }

    // ✅ Visualize spawn range in Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 top = new Vector3(transform.position.x, ySpawnRange.y, 0);
        Vector3 bottom = new Vector3(transform.position.x, ySpawnRange.x, 0);

        Gizmos.DrawLine(top, bottom);
        Gizmos.DrawSphere(top, 0.1f);
        Gizmos.DrawSphere(bottom, 0.1f);

#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.Label(top + Vector3.up * 0.2f, "Y Max");
        UnityEditor.Handles.Label(bottom - Vector3.up * 0.2f, "Y Min");
#endif
    }
}
