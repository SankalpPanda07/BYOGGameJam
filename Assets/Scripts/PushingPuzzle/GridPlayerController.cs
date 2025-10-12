using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerGridMove : MonoBehaviour
{
    public float moveTimePerTile = 0.12f;

    private Vector2Int currentCell;
    private bool isMoving = false;

    void Start()
    {
        // Snap to grid at start
        currentCell = GridManager.Instance.WorldToCell(transform.position);
        transform.position = GridManager.Instance.CellToWorld(currentCell);
        GridManager.Instance.RegisterObject(currentCell, gameObject);
    }

    void Update()
    {
        if (isMoving) return;

        Vector2Int dir = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) dir = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) dir = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) dir = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) dir = Vector2Int.right;

        if (dir == Vector2Int.zero) return;

        Vector2Int targetCell = currentCell + dir;
        GameObject obj = GridManager.Instance.GetObjectAt(targetCell);

        if (obj == null)
        {
            StartCoroutine(MoveToCell(targetCell));
        }
        else
        {
            Boulder b = obj.GetComponent<Boulder>();
            if (b != null && b.TryPush(dir))
                StartCoroutine(MoveToCell(targetCell));
        }
    }

    IEnumerator MoveToCell(Vector2Int targetCell)
    {
        isMoving = true;

        // Update grid occupancy
        GridManager.Instance.MoveRegistration(currentCell, targetCell, gameObject);

        Vector3 startPos = transform.position;
        Vector3 endPos = GridManager.Instance.CellToWorld(targetCell);
        float speed = Vector3.Distance(startPos, endPos) / moveTimePerTile;

        while ((Vector2)transform.position != (Vector2)endPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
            yield return null;
        }

        // Snap to exact position
        transform.position = endPos;
        currentCell = targetCell;
        isMoving = false;
    }
}
