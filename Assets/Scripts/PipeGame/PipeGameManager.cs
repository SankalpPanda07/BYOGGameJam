using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PipeEntry
{
    public Pipe pipe;                   // The pipe
    public PipeDirection[] validDirections; // Any of these directions is acceptable
}

public class PipeGameManager : MonoBehaviour
{
    public static PipeGameManager Instance;

    [Header("Pipes & Valid Directions")]
    public PipeEntry[] pipes; // single array of pipes, each with its valid directions

    private bool puzzleSolved = false;

    private void Awake()
    {
        Instance = this;
    }

    // Call this whenever a pipe is rotated
    public void CheckSolution()
    {
        if (puzzleSolved) return;

        foreach (var entry in pipes)
        {
            if (!ArrayContains(entry.validDirections, entry.pipe.currentDirection))
            {
                Debug.Log("❌ Not solved yet, keep trying!");
                return; // fail immediately if any pipe is incorrect
            }
        }

        puzzleSolved = true;
        Debug.Log("✅ Puzzle Solved!");
        GoToLevel(2); // example: load next level
    }

    private bool ArrayContains(PipeDirection[] arr, PipeDirection dir)
    {
        if (arr == null || arr.Length == 0) return false;
        foreach (var d in arr)
        {
            if (d == dir) return true; // any match is enough
        }
        return false;
    }

    private void GoToLevel(int level)
    {
        // Example: load scene by build index
        // SceneManager.LoadScene(level);
        Debug.Log($"🎮 Loading Level {level}...");
    }
}
