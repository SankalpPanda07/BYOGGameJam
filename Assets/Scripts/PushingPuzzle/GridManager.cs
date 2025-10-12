using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Grid Settings")]
    public Vector2 cellSize = Vector2.one;
    public Color gridColor = Color.green; // for scene view only

    private Dictionary<Vector2Int, GameObject> occupancy = new Dictionary<Vector2Int, GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Convert world position -> cell coordinates (no snapping to world)
    public Vector2Int WorldToCell(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x / cellSize.x);
        int y = Mathf.FloorToInt(worldPos.y / cellSize.y);
        return new Vector2Int(x, y);
    }

    // Convert cell coordinates -> world position (for snapping movement)
    public Vector3 CellToWorld(Vector2Int cell)
    {
        return new Vector3(
            cell.x * cellSize.x + cellSize.x * 0.5f,
            cell.y * cellSize.y + cellSize.y * 0.5f,
            0f
        );
    }

    // Check if cell is free
    public bool IsCellFree(Vector2Int cell)
    {
        return !occupancy.ContainsKey(cell) || occupancy[cell] == null;
    }

    // Get object at cell
    public GameObject GetObjectAt(Vector2Int cell)
    {
        occupancy.TryGetValue(cell, out GameObject go);
        return go;
    }

    // Register without snapping
    public void RegisterObject(Vector2Int cell, GameObject go)
    {
        occupancy[cell] = go;
    }

    // Unregister a cell
    public void UnregisterCell(Vector2Int cell)
    {
        if (occupancy.ContainsKey(cell)) occupancy.Remove(cell);
    }

    // Move registration when object moves between cells
    public void MoveRegistration(Vector2Int from, Vector2Int to, GameObject go)
    {
        if (occupancy.ContainsKey(from) && occupancy[from] == go)
            occupancy.Remove(from);
        occupancy[to] = go;
    }

    // Draw grid in Scene View only (not in Game View)
    private void OnDrawGizmos()
    {
        Gizmos.color = gridColor;

        // Draw a grid around the origin for visualization
        for (int x = -20; x <= 20; x++)
        {
            for (int y = -20; y <= 20; y++)
            {
                Vector3 cellCenter = CellToWorld(new Vector2Int(x, y));
                Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize.x, cellSize.y, 0));
            }
        }
    }
}
