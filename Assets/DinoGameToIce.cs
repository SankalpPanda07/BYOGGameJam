using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DinoGameToIce : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad = "IceAgeEra"; // set your target scene name here
    public float delay = 5f; // time before loading (in seconds)

    void Start()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }
   
    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }
}
