using System.Collections;
using UnityEngine;

public class NPCMorph : MonoBehaviour
{
    [Header("Sprite Settings")]
    public SpriteRenderer normalSprite;
    public SpriteRenderer deformedSprite;

    [Header("Morph Settings")]
    [Tooltip("How long the morph transition lasts (in seconds).")]
    public float transitionDuration = 1f;

    [Tooltip("Time before the morph starts (in seconds).")]
    public float morphDelay = 4f;

    private bool isMorphing = false;

    void Start()
    {
        // Start with only the normal sprite visible
        normalSprite.color = new Color(1, 1, 1, 1);
        deformedSprite.color = new Color(1, 1, 1, 0);

        // Auto-start morph after delay
        StartCoroutine(DelayedMorph());
    }

    private IEnumerator DelayedMorph()
    {
        yield return new WaitForSeconds(morphDelay);
        TriggerMorph();
    }

    public void TriggerMorph()
    {
        if (!isMorphing)
            StartCoroutine(MorphToDeformed());
    }

    private IEnumerator MorphToDeformed()
    {
        isMorphing = true;

        float t = 0f;
        while (t < transitionDuration)
        {
            t += Time.deltaTime;
            float alpha = t / transitionDuration;

            normalSprite.color = new Color(1, 1, 1, 1 - alpha);
            deformedSprite.color = new Color(1, 1, 1, alpha);

            yield return null;
        }

        normalSprite.color = new Color(1, 1, 1, 0);
        deformedSprite.color = new Color(1, 1, 1, 1);

        isMorphing = false;
    }
}
