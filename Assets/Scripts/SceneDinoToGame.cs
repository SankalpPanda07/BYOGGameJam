using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDinoToGame : MonoBehaviour
{
    [Header("UI Settings")]
    [Tooltip("Canvas to enable when player collides.")]
    public GameObject targetCanvas;

    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load.")]
    public string sceneToLoad;

    [Tooltip("How long (in seconds) the canvas stays open before changing scene.")]
    public float waitTime = 3f;

    private bool hasTriggered = false;

    private void Start()
    {
        targetCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(OpenCanvasAndChangeScene());
        }
    }

    private IEnumerator OpenCanvasAndChangeScene()
    {
        if (targetCanvas != null)
            targetCanvas.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        if (!string.IsNullOrEmpty(sceneToLoad))
            SceneManager.LoadScene(sceneToLoad);
        else
            Debug.LogWarning("No scene name specified in OpenCanvasAndLoadScene!");
    }
}
