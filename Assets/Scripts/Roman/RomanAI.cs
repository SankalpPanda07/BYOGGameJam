using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RomanAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;            // Assign the player transform
    public Transform sword;             // Sword pivot at handle

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float attackRange = 2f;      // Distance at which enemy attacks
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    [Header("Sword Settings")]
    public float swingSpeed = 600f;
    public float swingAngle = 120f;
    public float cooldownTime = 1f;

    private bool isSwinging = false;
    private bool canSwing = true;
    private float startAngle;
    private float targetAngle;
    private float currentSwingTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.gravityScale = 0; // Top-down, no gravity
        if (sword == null) Debug.LogError("Sword not assigned!");
        if (player == null) Debug.LogError("Player not assigned!");
    }

    void Update()
    {
        if (player == null) return;

        HandleFlip();
        HandleMovementAndAttack();

        if (isSwinging)
            RotateSword();
    }

    void FixedUpdate()
    {
        MoveEnemy();
    }

    // Flip enemy to face player
    void HandleFlip()
    {
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void HandleMovementAndAttack()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // Move toward player
            moveDirection = (player.position - transform.position).normalized;
        }
        else
        {
            // Stop moving and attack
            moveDirection = Vector2.zero;
            if (canSwing && !isSwinging)
                StartSwing();
        }
    }

    void MoveEnemy()
    {
        if (rb != null)
            rb.velocity = moveDirection * moveSpeed;
    }

    // 🟥 SWORD LOGIC
    void StartSwing()
    {
        isSwinging = true;
        canSwing = false;
        currentSwingTime = 0f;

        startAngle = -swingAngle / 2f;
        targetAngle = swingAngle / 2f;

        sword.localRotation = Quaternion.Euler(0, 0, startAngle);
    }

    void RotateSword()
    {
        currentSwingTime += Time.deltaTime * swingSpeed / swingAngle;
        float t = currentSwingTime;

        sword.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(startAngle, targetAngle, t));

        if (t >= 1f)
        {
            isSwinging = false;
            sword.localRotation = Quaternion.identity; // Reset rotation
            Invoke(nameof(ResetSwing), cooldownTime);
        }
    }

    void ResetSwing()
    {
        canSwing = true;
    }
}
