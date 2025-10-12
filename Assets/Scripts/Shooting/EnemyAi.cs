using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform weapon;
    private SpriteRenderer spriteRenderer;

    [Header("Movement Settings")]
    public float maxMoveSpeed = 3f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float patrolRange = 5f;
    public float minShootRange = 5f;
    public float maxShootRange = 10f;

    [Header("Shooting Settings")]
    public float fireRate = 1f;
    public float bulletSpeed = 10f;

    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    public int damageTakenPerHit = 25;

    private Vector3 startPosition;
    private Vector3 patrolTarget;
    private float fireTimer = 0f;

    private float currentSpeed = 0f;
    private Vector3 moveDirection = Vector3.zero;
    private bool waitingAtPatrolPoint = false;
    private bool playerSpotted = false;

    private enum State { Patrol, Chase, Shoot }
    private State currentState = State.Patrol;

    void Start()
    {
        startPosition = transform.position;
        SetNewPatrolTarget();

        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogWarning("EnemyAI: No SpriteRenderer found on this enemy!");
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Enemy detection
        if (distanceToPlayer <= maxShootRange * 1.5f)
            playerSpotted = true;

        // State logic
        if (playerSpotted)
        {
            if (distanceToPlayer >= minShootRange && distanceToPlayer <= maxShootRange)
                currentState = State.Shoot;
            else
                currentState = State.Chase;
        }
        else
        {
            currentState = State.Patrol;
        }

        switch (currentState)
        {
            case State.Patrol:
                if (!waitingAtPatrolPoint)
                    PatrolMovement();
                ResetWeaponRotation();
                break;

            case State.Chase:
                RotateWeaponTowards(player.position);
                FacePlayer(); // ✅ make sure enemy faces the player
                if (distanceToPlayer < minShootRange)
                    SetMoveDirection((transform.position - player.position).normalized);
                else
                    SetMoveDirection((player.position - transform.position).normalized);
                break;

            case State.Shoot:
                RotateWeaponTowards(player.position);
                FacePlayer(); // ✅ also face player when shooting
                ShootAtPlayer();
                if (distanceToPlayer < minShootRange)
                    SetMoveDirection((transform.position - player.position).normalized);
                else if (distanceToPlayer > maxShootRange)
                    SetMoveDirection((player.position - transform.position).normalized);
                else
                    SetMoveDirection(Vector3.zero);
                break;
        }

        ApplyMovement();
        fireTimer += Time.deltaTime;
    }

    // ✅ Makes enemy sprite + weapon face player
    void FacePlayer()
    {
        if (spriteRenderer == null || weapon == null) return;

        if (player.position.x < transform.position.x)
        {
            // Player is left → flip enemy + invert weapon Y
            spriteRenderer.flipX = true;
            weapon.localScale = new Vector3(weapon.localScale.x, -Mathf.Abs(weapon.localScale.y), weapon.localScale.z);
        }
        else
        {
            // Player is right → normal
            spriteRenderer.flipX = false;
            weapon.localScale = new Vector3(weapon.localScale.x, Mathf.Abs(weapon.localScale.y), weapon.localScale.z);
        }
    }

    // ---------- Movement ----------
    void PatrolMovement()
    {
        if (Vector3.Distance(transform.position, patrolTarget) < 0.1f)
        {
            StartCoroutine(WaitAtPatrolPoint());
            return;
        }

        SetMoveDirection((patrolTarget - transform.position).normalized);
    }

    IEnumerator WaitAtPatrolPoint()
    {
        waitingAtPatrolPoint = true;
        SetMoveDirection(Vector3.zero);
        yield return new WaitForSeconds(1f);
        SetNewPatrolTarget();
        waitingAtPatrolPoint = false;
    }

    void SetNewPatrolTarget()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRange;
        patrolTarget = startPosition + new Vector3(randomOffset.x, randomOffset.y, 0);
    }

    void SetMoveDirection(Vector3 direction) => moveDirection = direction;

    void ApplyMovement()
    {
        if (moveDirection.magnitude > 0.1f)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxMoveSpeed);
        }
        else
        {
            currentSpeed -= deceleration * Time.deltaTime;
            currentSpeed = Mathf.Max(currentSpeed, 0f);
        }

        transform.position += moveDirection * currentSpeed * Time.deltaTime;
    }

    // ---------- Combat ----------
    void ShootAtPlayer()
    {
        if (bulletPrefab == null || firePoint == null) return;

        if (fireTimer >= fireRate)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = bullet.transform.right * bulletSpeed;

            fireTimer = 0f;
        }
    }

    void RotateWeaponTowards(Vector3 target)
    {
        if (weapon == null) return;

        Vector3 direction = target - weapon.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weapon.localEulerAngles = new Vector3(0, 0, angle);
    }

    void ResetWeaponRotation()
    {
        if (weapon != null)
            weapon.localEulerAngles = Vector3.zero;
    }

    // ---------- Health ----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(damageTakenPerHit);
            Destroy(collision.gameObject); // remove bullet
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has been destroyed!");
        Destroy(gameObject);
    }

    // ---------- Gizmos ----------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxShootRange * 1.5f); // detection radius

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxShootRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minShootRange);
    }
}
