using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health
    public float currentHealth; // Current health
    public Slider healthBar; // Reference to the health bar UI (Slider)
    public float regenerationRate = 5f; // Amount of health regenerated per second
    public ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        // Initialize health
        currentHealth = maxHealth;

        // Update health bar
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    private void Update()
    {
        RegenerateHealth();
    }

    private void RegenerateHealth()
    {
        // Regenerate health over time
        if (currentHealth < maxHealth)
        {
            currentHealth += regenerationRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            // Update health bar
            if (healthBar != null)
            {
                healthBar.value = currentHealth;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        // Reduce health
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update health bar
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Check if the player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        scoreManager.Die();

    }
}
