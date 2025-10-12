using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavemenMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.gravityScale = 0; // No gravity in top-down
    }

    void Update()
    {
        HandleMovementInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    // 🟩 MOVEMENT LOGIC
    void HandleMovementInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // Flip the player using scale (facing direction)
        if (moveInput.x > 0)
            transform.localScale = new Vector3(2.5f, 2.5f, 1);
        else if (moveInput.x < 0)
            transform.localScale = new Vector3(-2.5f, 2.5f, 1);
    }

    void MovePlayer()
    {
        if (rb != null)
            rb.velocity = moveInput * moveSpeed;
    }
}