using UnityEngine;

public enum PipeDirection
{
    Up,
    Right,
    Down,
    Left
}

public class Pipe : MonoBehaviour
{
    public PipeDirection currentDirection;   // current orientation

    void OnMouseDown()
    {
        RotatePipe();
        PipeGameManager.Instance.CheckSolution();
    }

    private void RotatePipe()
    {
        // Rotate visually
        transform.Rotate(0, 0, -90f);

        // Rotate enum clockwise
        currentDirection = GetNextDirection(currentDirection);

        Debug.Log($"{gameObject.name} rotated → {currentDirection}");
    }

    private PipeDirection GetNextDirection(PipeDirection dir)
    {
        switch (dir)
        {
            case PipeDirection.Up: return PipeDirection.Right;
            case PipeDirection.Right: return PipeDirection.Down;
            case PipeDirection.Down: return PipeDirection.Left;
            case PipeDirection.Left: return PipeDirection.Up;
            default: return PipeDirection.Up;
        }
    }
}
