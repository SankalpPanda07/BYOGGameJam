using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject winPanel;
    // public GameObject losePanel;

    [Header("Game Timer")]
    public float gameDuration = 30f; // total time in seconds
    public TMP_Text timerText;       // Use TextMeshProUGUI

    [Header("Dinos")]
    public List<DinoAI> allDinos = new List<DinoAI>();

    private float currentTime;
    private bool gameEnded = false;

    void Start()
    {
        currentTime = gameDuration;

        if (winPanel != null) winPanel.SetActive(false);
        // if (losePanel != null) losePanel.SetActive(false);

        // Collect all existing dinos
        allDinos = new List<DinoAI>(FindObjectsOfType<DinoAI>());

        // Subscribe to death event
        foreach (var dino in allDinos)
        {
           // dino.OnDinoDeath += CheckDinosAlive;
        }

        StartCoroutine(GameTimerRoutine());
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = Mathf.CeilToInt(currentTime).ToString();
    }

    private IEnumerator GameTimerRoutine()
    {
        while (currentTime > 0f && !gameEnded)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
            yield return null;
        }

        if (!gameEnded)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        gameEnded = true;
        bool anyAlive = false;
        foreach (var dino in allDinos)
        {
            if (dino != null)
            {
                anyAlive = true;
                break;
            }
        }

        if (anyAlive)
        {
            if (winPanel != null) winPanel.SetActive(true);
        }
        else
        {
            // Lose scene
            SceneManager.LoadScene("Level6");
        }
    }

    private void CheckDinosAlive()
    {
        if (gameEnded) return;

        bool anyAlive = false;
        foreach (var dino in allDinos)
        {
            if (dino != null)
            {
                anyAlive = true;
                break;
            }
        }

        if (!anyAlive)
        {
            gameEnded = true;
            // Lose scene
            SceneManager.LoadScene("IceAgeEra");
        }
    }
}
