using UnityEngine;

public class BandookPlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;            // Movement speed

    [Header("References")]
    public Transform weapon;                // Weapon child (drag from hierarchy)

    private Rigidbody2D rb;                 // Rigidbody2D reference
    private Vector2 moveInput;              // Player input
    private SpriteRenderer spriteRenderer;  // Player sprite
    private Vector3 weaponInitialScale;     // Store weapon's base scale

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        weaponInitialScale = weapon.localScale;

        if (rb == null)
            Debug.LogError("Rigidbody2D missing on Player!");
        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer missing on Player!");
        if (weapon == null)
            Debug.LogError("Weapon Transform not assigned!");
    }

    void Update()
    {
        // Get player input (WASD / Arrow keys)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // Get mouse world position
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Rotate weapon toward mouse
        Vector2 direction = (mouseWorldPos - weapon.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weapon.rotation = Quaternion.Euler(0, 0, angle);

        // Flip logic: player via flipX, weapon via scaleY
        if (mouseWorldPos.x < transform.position.x)
        {
            spriteRenderer.flipX = true; // Flip player sprite
            weapon.localScale = new Vector3(weaponInitialScale.x, -Mathf.Abs(weaponInitialScale.y), weaponInitialScale.z);
        }
        else
        {
            spriteRenderer.flipX = false; // Normal
            weapon.localScale = new Vector3(weaponInitialScale.x, Mathf.Abs(weaponInitialScale.y), weaponInitialScale.z);
        }
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.velocity = moveInput * moveSpeed;
    }
}
