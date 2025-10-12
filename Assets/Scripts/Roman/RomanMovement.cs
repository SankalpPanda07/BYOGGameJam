using UnityEngine;

public class PlayerMovementAndAttack : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Sword Settings")]
    public Transform sword;             // Sword pivot should be at handle
    public float swingSpeed = 600f;     // Swing rotation speed
    public float swingAngle = 120f;     // How far the sword swings
    public float cooldownTime = 0.4f;   // Time between swings
    public GameObject swordCollider;    // Child GameObject for collision

    private bool isSwinging = false;
    private bool canSwing = true;
    private float startAngle;
    private float targetAngle;
    private float currentSwingTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.gravityScale = 0; // No gravity in top-down

        if (sword == null)
            Debug.LogError("Sword reference not assigned in Inspector!");
        if (swordCollider == null)
            Debug.LogError("Sword Collider reference not assigned in Inspector!");
        else
            swordCollider.SetActive(false); // Make sure collider starts OFF
    }

    void Update()
    {
        HandleMovementInput();
        HandleAttackInput();

        if (isSwinging)
            RotateSword();
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
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void MovePlayer()
    {
        if (rb != null)
            rb.velocity = moveInput * moveSpeed;
    }

    // 🟥 ATTACK LOGIC
    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && canSwing)
            StartSwing();
    }

    void StartSwing()
    {
        isSwinging = true;
        canSwing = false;
        currentSwingTime = 0f;

        // Enable sword collider
        if (swordCollider != null)
            swordCollider.SetActive(true);

        // Sword rotation from upward position to downward swing
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
            sword.localRotation = Quaternion.identity; // Reset to idle

            // Disable sword collider after swing
            if (swordCollider != null)
                swordCollider.SetActive(false);

            Invoke(nameof(ResetSwing), cooldownTime);
        }
    }

    void ResetSwing()
    {
        canSwing = true;
    }
}
