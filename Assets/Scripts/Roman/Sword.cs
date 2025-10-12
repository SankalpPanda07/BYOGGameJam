using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Sword : MonoBehaviour
{
    [Header("Attack Settings")]
    public int damage = 25;
    public float attackCooldown = 0.5f;

    private bool canAttack = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canAttack) return;

        if (other.CompareTag("Enemy"))
        {
            // Deal damage
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("⚔️ Hit enemy for " + damage);
            }
        }
    }

    public void Attack()
    {
        if (canAttack)
        {
            Debug.Log("🗡️ Player swings sword!");
            // Trigger animation here
            StartCoroutine(AttackCooldown());
        }
    }

    private System.Collections.IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
