using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    private int score = 0;          
    private float timer = 0f;        
    private bool isRunning = true;   

    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;

            //1 is normal time, the lesser time, timer will fast up
            if (timer >= 0.2f)
            {
                timer = 0f;
                score++;   
                UpdateScoreText();
            }
        }


        //testing

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Die();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {score:0000}"; 
    }

    public void StopScore()
    {
        isRunning = false;
    }

    public void ResetScore()
    {
        score = 0;
        timer = 0f;
        UpdateScoreText();
    }

    // Call this function to handle player death
    public void Die()
    {
        StopScore(); 
        PlayerPrefs.SetInt("FinalScore", score);
        int highScore = PlayerPrefs.GetInt("HighScore", 0); 
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score); 
        }

        SceneManager.LoadScene("Death"); 
    }

}
