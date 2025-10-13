using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DinoAI : MonoBehaviour
{
    public event System.Action OnDinoDeath;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float directionChangeInterval = 2f; // Time before changing direction
    private Vector2 moveDirection;

    [Header("Rescue Zone Avoidance")]
    public string rescueZoneTag = "RescueZone";
    public float avoidStrength = 5f;

    [Header("World Bounds")]
    public Vector2 minBounds = new Vector2(-10f, -5f);
    public Vector2 maxBounds = new Vector2(10f, 5f);

    [Header("Health Settings")]
    public int maxHealth = 30;
    public int currentHealth;
    public Slider healthSlider;

    [Header("Damage Settings")]
    public string damageTag = "Asteroid";
    public float damageInterval = 5f;
    public int damagePerTick = 1;
    private HashSet<Collider2D> damagingColliders = new HashSet<Collider2D>();

    private Rigidbody2D rb;
    private float nextDirectionChangeTime;
    private bool isRunning = false;
    private bool isPaused = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        PickRandomDirection();
    }

    private void Update()
    {
        if (!isPaused && Time.time >= nextDirectionChangeTime)
            PickRandomDirection();

        AvoidRescueZones();
        KeepInsideBounds();

        // move
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        // rotate to face movement direction
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void PickRandomDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
        nextDirectionChangeTime = Time.time + directionChangeInterval;
    }

    void AvoidRescueZones()
    {
        Collider2D[] zones = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (var zone in zones)
        {
            if (zone.CompareTag(rescueZoneTag))
            {
                Vector2 away = ((Vector2)transform.position - (Vector2)zone.transform.position).normalized;
                moveDirection = Vector2.Lerp(moveDirection, away, 0.2f).normalized;
            }
        }
    }

    void KeepInsideBounds()
    {
        Vector2 pos = transform.position;
        if (pos.x < minBounds.x) moveDirection.x = Mathf.Abs(moveDirection.x);
        else if (pos.x > maxBounds.x) moveDirection.x = -Mathf.Abs(moveDirection.x);

        if (pos.y < minBounds.y) moveDirection.y = Mathf.Abs(moveDirection.y);
        else if (pos.y > maxBounds.y) moveDirection.y = -Mathf.Abs(moveDirection.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(damageTag) && !damagingColliders.Contains(other))
        {
            damagingColliders.Add(other);
            StartCoroutine(ApplyContinuousDamage(other));
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(damageTag) && !damagingColliders.Contains(other))
        {
            damagingColliders.Add(other);
            StartCoroutine(ApplyContinuousDamage(other));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(damageTag) && damagingColliders.Contains(other))
            damagingColliders.Remove(other);
    }

    private IEnumerator ApplyContinuousDamage(Collider2D asteroid)
    {
        while (damagingColliders.Contains(asteroid))
        {
            TakeDamage(damagePerTick);
            yield return new WaitForSeconds(damageInterval);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        Debug.Log($"{gameObject.name} took {damage} damage. Remaining HP: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        OnDinoDeath?.Invoke();
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((minBounds + maxBounds) / 2f, maxBounds - minBounds);
    }
}
