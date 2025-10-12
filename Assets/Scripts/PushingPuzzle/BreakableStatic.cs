using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BreakableStatic : MonoBehaviour
{
    [Tooltip("Prefab to spawn after destruction")]
    public GameObject spawnPrefab;

    void Start()
    {
        // Register like a StaticOccupier so it blocks player movement
        var cell = GridManager.Instance.WorldToCell(transform.position);
        GridManager.Instance.RegisterObject(cell, gameObject);
        transform.position = GridManager.Instance.CellToWorld(cell);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Boulder boulder = other.GetComponent<Boulder>();
        if (boulder != null)
        {
            BreakBoulder(boulder);
        }
    }

    // 👇 make this PUBLIC so Boulder.cs can call it
    public void BreakBoulder(Boulder boulder)
    {
        if (boulder != null)
            Destroy(boulder.gameObject);

        Destroy(gameObject);

    }
}
