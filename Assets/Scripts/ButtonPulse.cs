using UnityEngine;

public class ButtonPulse : MonoBehaviour
{
    [Tooltip("How much the button scales up (e.g., 1.1 = 10% bigger).")]
    public float scaleMultiplier = 1.1f;

    [Tooltip("How fast it zooms in/out.")]
    public float pulseSpeed = 2f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * (scaleMultiplier - 1);
        transform.localScale = originalScale * scale;
    }
}
