using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public GameObject HomePanel;
    public GameObject OptionPanel;

    public GameObject CreditsPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        HomePanel.SetActive(true);
        OptionPanel.SetActive(false);

        CreditsPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Level0");
    }
    public void Tutorial()
    {

        HomePanel.SetActive(false);
        OptionPanel.SetActive(false);
  
        CreditsPanel.SetActive(false);
    }

    public void TutorialSkip()
    {
        SceneManager.LoadScene("Level0");
    }

    public void Options()
    {

        HomePanel.SetActive(false);
        OptionPanel.SetActive(true);

        CreditsPanel.SetActive(false);
    }

    public void OptionsBack()
    {

        HomePanel.SetActive(true);
        OptionPanel.SetActive(false);

        CreditsPanel.SetActive(false);
    }

    public void Controls()
    {

        HomePanel.SetActive(false);
        OptionPanel.SetActive(false);

        CreditsPanel.SetActive(false);
    }

    public void ControlsBack()
    {

        HomePanel.SetActive(false);
        OptionPanel.SetActive(true);

        CreditsPanel.SetActive(false);
    }

    public void Credits()
    {

        HomePanel.SetActive(false);
        OptionPanel.SetActive(false);

        CreditsPanel.SetActive(true);
    }

    public void CreditsBack()
    {

        HomePanel.SetActive(false);
        OptionPanel.SetActive(true);
        CreditsPanel.SetActive(false);
    }

    public void exit()
    {
        Application.Quit();
    }

}
