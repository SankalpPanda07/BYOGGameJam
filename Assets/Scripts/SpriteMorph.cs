using UnityEngine;
using System.Collections;

public class SpriteMorph : MonoBehaviour
{
    public SpriteRenderer normalSprite;
    public SpriteRenderer deformedSprite;
    public float transitionDuration = 1f; // seconds

    private bool isMorphing = false;

    void Start()
    {
        // Start with only the normal sprite visible
        normalSprite.color = new Color(1, 1, 1, 1);
        deformedSprite.color = new Color(1, 1, 1, 0);
    }


    public void TriggerMorph()
    {
        if (!isMorphing)
            StartCoroutine(MorphToDeformed());
    }

    IEnumerator MorphToDeformed()
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