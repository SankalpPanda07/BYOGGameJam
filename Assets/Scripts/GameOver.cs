using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public ScoreManager scoreManager;

    public void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Hell")
        {
            scoreManager.Die();
        }
    }
}
