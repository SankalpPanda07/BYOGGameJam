using UnityEngine;
using System.Collections;

public class NPCWalkAndDissolve : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Movement direction. Set (1,0)=Right, (-1,0)=Left, (0,1)=Up, (0,-1)=Down.")]
    public Vector2 moveDirection = Vector2.right;
    public float moveSpeed = 2f;

    [Header("Dissolve Settings")]
    [Tooltip("Time (seconds) before dissolve starts.")]
    public float dissolveDelay = 5f;

    [Tooltip("How long the dissolve effect lasts (seconds).")]
    public float dissolveDuration = 2f;

    [Header("Sprite Settings")]
    [Tooltip("If your sprite is a child, assign its SpriteRenderer here.")]
    public SpriteRenderer childSpriteRenderer;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isDissolving = false;

    void Start()
    {
        // Use assigned child sprite, or auto-find one if not assigned
        spriteRenderer = childSpriteRenderer ?? GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("NPCWalkAndDissolve: No SpriteRenderer found! Assign manually.");
            enabled = false;
            return;
        }

        originalColor = spriteRenderer.color;
        moveDirection = moveDirection.normalized;

        StartCoroutine(DissolveAfterDelay());
    }

    void Update()
    {
        if (!isDissolving)
        {
            // Move NPC continuously
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    private IEnumerator DissolveAfterDelay()
    {
        yield return new WaitForSeconds(dissolveDelay);
        StartCoroutine(DissolveAndDestroy());
    }

    private IEnumerator DissolveAndDestroy()
    {
        isDissolving = true;

        float t = 0f;
        while (t < dissolveDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / dissolveDuration);

            Color c = originalColor;
            c.a = alpha;
            spriteRenderer.color = c;

            yield return null;
        }

        Destroy(gameObject);
    }
}
