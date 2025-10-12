using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public class DinoAI : MonoBehaviour
{
    public event System.Action OnDinoDeath;


    [Header("Movement Settings")]
    public float normalSpeed = 2f;
    public float runSpeed = 5f;
    public float changeDirectionInterval = 2f; // Time before changing direction
    private Vector2 moveDirection;
    private bool isRunning = false;

    [Header("Health Settings")]
    public int maxHealth = 30;
    public int currentHealth;
    public Slider healthSlider;

    [Header("Pause Settings")]
    public float minPauseTime = 1f;
    public float maxPauseTime = 2f;
    private bool isPaused = false;

    [Header("Avoid Zones")]
    public string avoidTag = "RescueZone";
    private List<Collider2D> zones = new List<Collider2D>();

    [Header("Damage Settings")]
    public string damageTag = "Asteroid";
    public float damageInterval = 0.5f;
    public int damagePerTick = 1;
    private HashSet<Collider2D> damagingColliders = new HashSet<Collider2D>();

    [Header("Separation Settings")]
    public float separationDistance = 1.5f; // minimum distance between dinos
    public float separationStrength = 1f; // how strongly they avoid each other
    private static List<DinoAI> allDinos = new List<DinoAI>();

    private Rigidbody2D rb;
    private Camera mainCamera;
    private float nextDirectionChangeTime;

    private void Awake()
    {
        allDinos.Add(this);
    }

    private void OnDestroy()
    {
        allDinos.Remove(this);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        ChooseNewDirection();
    }

    private void Update()
    {
        if (!isPaused && Time.time >= nextDirectionChangeTime && !isRunning)
        {
            StartCoroutine(PauseAndChooseDirection());
        }
    }

    private void FixedUpdate()
    {
        if (!isPaused)
        {
            ApplySeparation();
            Move();
            RotateTowardsMovement(); // Rotate to face movement direction
        }
    }

    private void Move()
    {
        float speed = isRunning ? runSpeed : normalSpeed;
        Vector2 newPos = rb.position + moveDirection * speed * Time.fixedDeltaTime;

        // Clamp to camera bounds
        Vector3 minBounds = mainCamera.ViewportToWorldPoint(Vector3.zero);
        Vector3 maxBounds = mainCamera.ViewportToWorldPoint(Vector3.one);
        newPos.x = Mathf.Clamp(newPos.x, minBounds.x, maxBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, minBounds.y, maxBounds.y);

        rb.MovePosition(newPos);
    }

    private void ApplySeparation()
    {
        Vector2 repulsion = Vector2.zero;
        foreach (var other in allDinos)
        {
            if (other == this) continue;
            float distance = Vector2.Distance(transform.position, other.transform.position);
            if (distance < separationDistance)
            {
                repulsion += ((Vector2)transform.position - (Vector2)other.transform.position).normalized
                             * (separationDistance - distance) * separationStrength;
            }
        }

        if (repulsion != Vector2.zero)
        {
            moveDirection = (moveDirection + repulsion).normalized;
        }
    }

    private void RotateTowardsMovement()
    {
        if (moveDirection != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            rb.rotation = targetAngle;
        }
    }

    private void ChooseNewDirection()
    {
        int attempts = 0;
        Vector2 newDir;
        do
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            newDir = new Vector2(x, y).normalized;
            attempts++;
        }
        while (IsDirectionIntoZone(newDir) && attempts < 10);

        moveDirection = newDir;
        nextDirectionChangeTime = Time.time + changeDirectionInterval;
    }

    private bool IsDirectionIntoZone(Vector2 dir)
    {
        Vector2 projectedPos = rb.position + dir * normalSpeed * Time.fixedDeltaTime * 5f; // look ahead
        foreach (var zone in zones)
        {
            if (zone != null && zone.bounds.Contains(projectedPos))
                return true;
        }
        return false;
    }

    private IEnumerator PauseAndChooseDirection()
    {
        isPaused = true;
        float pauseTime = Random.Range(minPauseTime, maxPauseTime);
        yield return new WaitForSeconds(pauseTime);
        ChooseNewDirection();
        isPaused = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Rescue Zone
        if (other.CompareTag(avoidTag))
        {
            if (!zones.Contains(other))
                zones.Add(other);
            StartCoroutine(EscapeFromZone(other));
        }

        // Asteroid damage
        if (other.CompareTag(damageTag))
        {
            if (!damagingColliders.Contains(other))
            {
                damagingColliders.Add(other);
                StartCoroutine(ApplyContinuousDamage(other));
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Rescue Zone
        if (other.CompareTag(avoidTag) && !isRunning)
        {
            StartCoroutine(EscapeFromZone(other));
        }

        // Asteroid damage
        if (other.CompareTag(damageTag))
        {
            if (!damagingColliders.Contains(other))
            {
                damagingColliders.Add(other);
                StartCoroutine(ApplyContinuousDamage(other));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(damageTag) && damagingColliders.Contains(other))
        {
            damagingColliders.Remove(other);
        }
    }

    private IEnumerator EscapeFromZone(Collider2D zone)
    {
        isRunning = true;
        Vector2 escapeDirection = ((Vector2)transform.position - (Vector2)zone.bounds.center).normalized;

        while (zone.bounds.Contains(transform.position))
        {
            rb.MovePosition(rb.position + escapeDirection * runSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        isRunning = false;
        ChooseNewDirection();
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

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        allDinos.Remove(this);
        OnDinoDeath?.Invoke(); // notify GameManager
        Destroy(gameObject);
    }
}
