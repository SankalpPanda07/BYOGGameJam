using UnityEngine;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
    [Header("UI Components")]
    [Tooltip("The UI Image for the oxygen bar. Must have its Image Type set to 'Filled' and Fill Method to 'Vertical'.")]
    public Image oxygenBar;

    [Tooltip("The Retry Canvas that appears when the player loses.")]
    public GameObject retryCanvas;

    [Tooltip("Tap to Start Canvas (contains the start button).")]
    public GameObject startCanvas;
    public GameObject TutCanvas;

    [Header("Oxygen Settings")]
    [Tooltip("The rate at which oxygen depletes per second. (e.g., 0.05 means 5% of the bar drains per second).")]
    public float depletionRate = 0.05f;

    [Tooltip("The amount of oxygen restored with a single button press. (e.g., 0.1 means 10% of the bar is refilled).")]
    public float refillAmount = 0.1f;

    private float currentOxygen;
    private bool gameStarted = false;
    private bool gameWon = false;
    private bool gameLost = false;

    public SceneTransitionManager sceneTransitionManager;

    void Start()
    {
        if (oxygenBar == null)
        {
            Debug.LogError("FATAL ERROR: The Oxygen Bar Image has not been assigned in the Inspector!");
            this.enabled = false;
            return;
        }

        if (retryCanvas != null)
            retryCanvas.SetActive(false);

        if (startCanvas != null)
            startCanvas.SetActive(true); // Show start button

        currentOxygen = 0.35f;
        oxygenBar.fillAmount = currentOxygen;

        // Stop game logic until started
        Time.timeScale = 0f;
        TutCanvas.SetActive(false);
    }

    void Update()
    {
        if (!gameStarted || gameWon || gameLost)
            return;

        currentOxygen -= depletionRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp01(currentOxygen);
        oxygenBar.fillAmount = currentOxygen;

        if (currentOxygen <= 0f)
            GameLost();
    }

    public void StartGame()
    {
        if (gameStarted) return;

        gameStarted = true;
        Time.timeScale = 1f;

        if (startCanvas != null)
            startCanvas.SetActive(false);

        Debug.Log("Game started!");

        TutCanvas.SetActive(true);
    }

    public void RefillOxygen()
    {
        if (!gameStarted || gameWon || gameLost)
            return;

        currentOxygen += refillAmount;

        if (currentOxygen >= 1f)
        {
            gameWon = true;
            currentOxygen = 1f;
            Debug.Log("We won the game!");

            sceneTransitionManager.ChangeScene("Level3");
        }

        oxygenBar.fillAmount = currentOxygen;
    }

    private void GameLost()
    {
        gameLost = true;
        currentOxygen = 0f;
        oxygenBar.fillAmount = currentOxygen;

        Time.timeScale = 0f;

        if (retryCanvas != null)
            retryCanvas.SetActive(true);

        Debug.Log("Game Over! You lost the game.");
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
