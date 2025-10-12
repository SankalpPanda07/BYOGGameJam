using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;       // Prefab of the bullet
    public float bulletSpeed = 10f;       // Speed of the bullet
    public Transform firePoint;           // Point from which bullets spawn

    [Header("Shooting Settings")]
    public float fireRate = 0.2f;         // Time between shots
    private float fireTimer = 0f;

    [Header("Player References")]
    public SpriteRenderer spriteRenderer; // To flip player sprite

    void Update()
    {
        // Flip player based on cursor
        FlipPlayerWithCursor();

        // Handle shooting input
        fireTimer += Time.deltaTime;
        if (Input.GetMouseButton(0) && fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    void Shoot()
    {
        // Spawn bullet at firePoint position
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Calculate rotation toward mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set bullet rotation
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Set bullet velocity along its local X-axis
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = bullet.transform.right * bulletSpeed;
        }
    }

    void FlipPlayerWithCursor()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.position.x)
            spriteRenderer.flipX = false; // Facing right
        else
            spriteRenderer.flipX = true;  // Facing left
    }
}
