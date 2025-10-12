using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText; 
    public TextMeshProUGUI highScoreText; 

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0); 
        int highScore = PlayerPrefs.GetInt("HighScore", 0);   

        finalScoreText.text = $"Your Score: {finalScore:0000}"; 
        highScoreText.text = $"High Score: {highScore:0000}";  
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay"); 
    }

    public void Home()
    {
        SceneManager.LoadScene("Home");
    }
}
