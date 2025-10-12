using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject PauseMenu;

    [SerializeField] public KeyCode PauseKey = KeyCode.Escape;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        PauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(PauseKey))
        {
            Time.timeScale = 0f;
            PauseMenu.SetActive(true);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);

    }

    public void Resume()
    {
        Time .timeScale = 1.0f;
        PauseMenu.SetActive(false);

    }
    public void Retry()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void Home()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Home");
    }


}
