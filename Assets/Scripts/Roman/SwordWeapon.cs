using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SwordWeapon : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 25;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player sword can hit Enemy
        if (gameObject.CompareTag("PlayerSword") && collision.CompareTag("Enemy"))
        {
            SwordEnemyHealth enemyHealth = collision.GetComponent<SwordEnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"⚔️ Player hit Enemy for {damage} damage!");
            }
        }

        // Enemy sword can hit Player
        else if (gameObject.CompareTag("EnemySword") && collision.CompareTag("Player"))
        {
            SwordPlayerHealth playerHealth = collision.GetComponent<SwordPlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"⚔️ Enemy hit Player for {damage} damage!");
            }
        }
    }
}
