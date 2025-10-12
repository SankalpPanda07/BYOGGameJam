using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level9Lose : MonoBehaviour
{
    [Header("Settings")]
    public float delayBeforeLoad = 3f; // seconds to wait before scene loads

    private void Start()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene("CaveMen");
    }
}
