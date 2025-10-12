using UnityEngine;

public class SwordEnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 50;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log($"🗡️ {gameObject.name} Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject);
    }
}
