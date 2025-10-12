using UnityEngine;

public class Gate : MonoBehaviour
{
    [Header("Step Blocks Required to Open Gate")]
    public GameObject[] requiredStepBlocks; // Assign all blocks in inspector

    void Update()
    {
        if (AllBlocksDestroyed())
        {
            DestroyGate();
        }
    }

    // Check if all required step blocks have been destroyed
    bool AllBlocksDestroyed()
    {
        foreach (GameObject block in requiredStepBlocks)
        {
            if (block != null)
                return false; // At least one block still exists
        }
        return true;
    }

    // Destroy the gate
    void DestroyGate()
    {
        // Optional: add a sound or animation here
        Destroy(gameObject);
    }
}
