using UnityEngine;
using UnityEngine.SceneManagement;

public class SwordGameManager : MonoBehaviour
{
    private SwordEnemyHealth[] enemies;

    private void Start()
    {
        enemies = FindObjectsOfType<SwordEnemyHealth>();
        Debug.Log($"🗡️ Total enemies at start: {enemies.Length}");
    }

    private void Update()
    {
        enemies = FindObjectsOfType<SwordEnemyHealth>();
        if (enemies.Length == 0)
        {
            Debug.Log("✅ All enemies have been killed! Reloading scene...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
