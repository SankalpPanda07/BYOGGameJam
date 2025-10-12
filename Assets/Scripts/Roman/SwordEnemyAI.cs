using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SwordEnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float stopDistance = 1.5f;  // Distance to stop before attacking

    [Header("Attack Settings")]
    public Transform weaponHolder;     // Enemy's sword pivot
    public float swingAngle = 90f;     // Total swing angle
    public float swingSpeed = 400f;    // Swing rotation speed
    public float attackCooldown = 2f;  // Time between attacks

    [Header("References")]
    public Transform player;           // Assign Player Transform in Inspector

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private bool isSwinging = false;
    private float targetRotation;
    private float startRotation;
    private float attackTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
            Debug.LogWarning("⚠️ SwordEnemyAI: Player reference missing!");
    }

    private void Update()
    {
        if (player == null) return;

        attackTimer -= Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance && !isSwinging)
        {
            // Move towards player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;

            SetFacing(direction.x);
        }
        else
        {
            // Stop moving and attack
            rb.velocity = Vector2.zero;

            if (!isSwinging && attackTimer <= 0f)
            {
                StartSwing();
                attackTimer = attackCooldown;
            }
        }

        // Handle swinging
        if (isSwinging)
        {
            float step = swingSpeed * Time.deltaTime;
            weaponHolder.localRotation = Quaternion.RotateTowards(
                weaponHolder.localRotation,
                Quaternion.Euler(0, 0, targetRotation),
                step
            );

            // Reset after swing
            if (Mathf.Abs(Mathf.DeltaAngle(weaponHolder.localEulerAngles.z, targetRotation)) < 1f)
            {
                isSwinging = false;
                weaponHolder.localRotation = Quaternion.Euler(0, 0, startRotation);
            }
        }
    }

    /// <summary>
    /// Flip enemy sprite based on player's position and also flip weapon X scale.
    /// </summary>
    void SetFacing(float dirX)
    {
        bool facingLeft = dirX < 0;

        if (spriteRenderer != null)
            spriteRenderer.flipX = facingLeft;

        if (weaponHolder != null)
        {
            Vector3 weaponScale = weaponHolder.localScale;
            weaponScale.x = facingLeft ? -Mathf.Abs(weaponScale.x) : Mathf.Abs(weaponScale.x);
            weaponHolder.localScale = weaponScale;
        }
    }

    /// <summary>
    /// Start sword swing (attack).
    /// </summary>
    void StartSwing()
    {
        isSwinging = true;
        startRotation = weaponHolder.localEulerAngles.z;

        if (spriteRenderer.flipX)
            targetRotation = startRotation - swingAngle; // Left → clockwise
        else
            targetRotation = startRotation + swingAngle; // Right → counter-clockwise
    }
}
