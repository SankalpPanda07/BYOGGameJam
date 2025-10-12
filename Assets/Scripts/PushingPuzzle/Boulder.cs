using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Boulder : MonoBehaviour
{
    Vector2Int currentCell;
    bool isMoving = false;

    void Start()
    {
        currentCell = GridManager.Instance.WorldToCell(transform.position);
        transform.position = GridManager.Instance.CellToWorld(currentCell);
        GridManager.Instance.RegisterObject(currentCell, gameObject);
    }

    // Called by player to push the boulder
    public bool TryPush(Vector2Int dir)
    {
        if (isMoving) return false;

        Vector2Int next = currentCell + dir;
        GameObject nextObj = GridManager.Instance.GetObjectAt(next);

        // If next cell has a breakable, snap into it and destroy both
        if (nextObj != null)
        {
            BreakableStatic bs = nextObj.GetComponent<BreakableStatic>();
            if (bs != null)
            {
                MoveToCell(next); // snap into the breakable
                bs.BreakBoulder(this); // destroy both
                return true;
            }

            // Block if static wall or another boulder
            if (nextObj.GetComponent<StaticOccupier>() != null || nextObj.GetComponent<Boulder>() != null)
                return false;
        }

        // Block if cell is otherwise occupied
        if (!GridManager.Instance.IsCellFree(next)) return false;

        // Move normally
        MoveToCell(next);
        return true;
    }

    // Snaps boulder to a specific cell
    void MoveToCell(Vector2Int targetCell)
    {
        isMoving = true;

        // Snap to position
        transform.position = GridManager.Instance.CellToWorld(targetCell);

        // Update grid registration
        GridManager.Instance.RegisterObject(targetCell, gameObject);
        GridManager.Instance.UnregisterCell(currentCell);

        currentCell = targetCell;
        isMoving = false;
    }

    // Trigger logic in case the boulder overlaps breakable via physics
    void OnTriggerEnter2D(Collider2D other)
    {
        BreakableStatic bs = other.GetComponent<BreakableStatic>();
        if (bs != null)
        {
            bs.BreakBoulder(this);
        }
    }
}
