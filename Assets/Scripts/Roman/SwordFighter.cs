using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SwordFighter : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Weapon Settings")]
    public Transform weaponHolder;     // The sword parent (pivot)
    public float swingAngle = 90f;     // Total swing angle
    public float swingSpeed = 500f;    // How fast the swing happens

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 movement;

    private bool isSwinging = false;
    private float targetRotation;
    private float startRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            Debug.LogWarning("⚠️ No SpriteRenderer found on player.");
    }

    private void Update()
    {
        // Movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Flip when moving left or right
        if (movement.x < 0)
            SetFacing(true);
        else if (movement.x > 0)
            SetFacing(false);

        // Sword swing on left mouse click
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartSwing();
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

            // Check if swing completed
            if (Mathf.Abs(Mathf.DeltaAngle(weaponHolder.localEulerAngles.z, targetRotation)) < 1f)
            {
                isSwinging = false;
                weaponHolder.localRotation = Quaternion.Euler(0, 0, startRotation);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = movement.normalized * moveSpeed;
    }

    /// <summary>
    /// Flip player sprite and adjust sword orientation.
    /// </summary>
    void SetFacing(bool facingLeft)
    {
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
    /// Starts a sword swing (rotation).
    /// </summary>
    void StartSwing()
    {
        isSwinging = true;

        startRotation = weaponHolder.localEulerAngles.z;
        if (spriteRenderer.flipX)
        {
            // Facing left → swing clockwise
            targetRotation = startRotation - swingAngle;
        }
        else
        {
            // Facing right → swing counter-clockwise
            targetRotation = startRotation + swingAngle;
        }
    }
}
