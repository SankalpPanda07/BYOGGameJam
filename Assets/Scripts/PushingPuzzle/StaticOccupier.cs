using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StaticOccupier : MonoBehaviour
{
    private Vector2Int cell;

    void Start()
    {
        // Only register position, don’t change it
        cell = GridManager.Instance.WorldToCell(transform.position);
        GridManager.Instance.RegisterObject(cell, gameObject);

        // ❌ Removed snapping line:
        // transform.position = GridManager.Instance.CellToWorld(cell);
    }
}
