using UnityEngine;

[ExecuteAlways]
public class GridSnapper : MonoBehaviour
{
    public Vector2 cellSize = Vector2.one;  // Size of grid cells
    public bool snapInEditor = true;        // Toggle snapping in editor
    public bool snapOnPlay = true;          // Snap once when Play starts
    private bool snappedOnPlayDone = false; // To ensure we snap only once

    void Update()
    {
        if (!Application.isPlaying && snapInEditor)
        {
            SnapToGrid();
        }
        else if (Application.isPlaying && snapOnPlay && !snappedOnPlayDone)
        {
            SnapToGrid();
            snappedOnPlayDone = true;
        }
    }

    void SnapToGrid()
    {
        Vector3 pos = transform.position;

        // Snap X
        int cellX = Mathf.FloorToInt(pos.x / cellSize.x);
        pos.x = cellX * cellSize.x + cellSize.x * 0.5f;

        // Snap Y
        int cellY = Mathf.FloorToInt(pos.y / cellSize.y);
        pos.y = cellY * cellSize.y + cellSize.y * 0.5f;

        transform.position = pos;
    }
}
