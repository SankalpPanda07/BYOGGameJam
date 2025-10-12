using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for Image component
using UnityEngine.SceneManagement; // Required for scene management

/// <summary>
/// Manages scene transitions with a fade-to-black effect.
/// Handles automatic fade-out on scene load and can be called to fade-in and change scenes.
/// Can also trigger a scene change automatically after a set time.
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    [Header("Transition Settings")]
    [Tooltip("Assign a single black UI Image used for fading in and out.")]
    public Image transitionImage;
    [Tooltip("The duration of the fade-in effect (to black).")]
    public float fadeInDuration = 2f;
    [Tooltip("The duration of the fade-out effect (from black).")]
    public float fadeOutDuration = 1f;

    [Header("Automatic Timed Transition")]
    [Tooltip("Check this to enable an automatic scene change after a delay.")]
    public bool useTimedTransition = false;
    [Tooltip("The delay in seconds before the scene transition starts.")]
    public float sceneChangeDelay = 20f;
    [Tooltip("The name of the scene to load automatically.")]
    public string nextSceneName;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        if (transitionImage == null)
        {
            Debug.LogError("Error: Transition Image has not been assigned in the Inspector on the SceneTransitionManager!");
            this.enabled = false; // Disable script if image is missing
            return;
        }

        // Start the fade-out effect for the new scene.
        StartCoroutine(Fade(1f, 0f, fadeOutDuration));

        // Check if a timed transition should be initiated.
        if (useTimedTransition)
        {
            if (string.IsNullOrEmpty(nextSceneName))
            {
                Debug.LogWarning("Timed Transition is enabled, but Next Scene Name is not set.");
            }
            else
            {
                StartCoroutine(TimedSceneChange());
            }
        }
    }


    /// <summary>
    /// Public function to be called from other scripts to change scenes.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    /// <summary>
    /// Coroutine that fades in to black and then loads the specified scene.
    /// </summary>
    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        yield return StartCoroutine(Fade(0f, 1f, fadeInDuration));
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// A generic coroutine to fade the transition image's alpha over time.
    /// </summary>
    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        transitionImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Color color = transitionImage.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            transitionImage.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        // Ensure the final alpha is set correctly.
        transitionImage.color = new Color(color.r, color.g, color.b, endAlpha);

        // Disable the image if it's fully transparent.
        if (endAlpha == 0f)
        {
            transitionImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Coroutine to handle the timed scene change.
    /// </summary>
    private IEnumerator TimedSceneChange()
    {
        // Wait for the specified delay.
        yield return new WaitForSeconds(sceneChangeDelay);

        Debug.Log($"Timed transition starting for scene: {nextSceneName}");
        // Call the public change scene method.
        ChangeScene(nextSceneName);
    }
}