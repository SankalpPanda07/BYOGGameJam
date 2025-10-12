using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellToRepAnim : MonoBehaviour
{
    private bool hasTriggered = false;

    public SpriteMorph SpriteMorph;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true; // Prevent future triggers
            SpriteMorph.TriggerMorph();
        }
    }
}
