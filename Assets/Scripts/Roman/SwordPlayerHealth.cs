using UnityEngine;
using UnityEngine.SceneManagement;

public class SwordPlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        PrintHealth();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        PrintHealth();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void PrintHealth()
    {
        Debug.Log($"🗡️ Player Health: {currentHealth}/{maxHealth}");
    }

    private void Die()
    {
        Debug.Log("💀 Player has died! Reloading scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
