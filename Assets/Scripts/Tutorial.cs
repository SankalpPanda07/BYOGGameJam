using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TutorialStep
{
    [Tooltip("The text to be displayed for this tutorial step.")]
    [TextArea(3, 10)]
    public string tutorialMessage;

    [Tooltip("The delay in seconds before this message appears.")]
    public float startDelay = 5f;

    [Tooltip("How long the message stays on screen (in seconds) after appearing.")]
    public float displayDuration = 1f;

    [Header("Audio Settings")]
    [Tooltip("Optional: Audio clip to play when this message appears.")]
    public AudioClip dialogueAudio;

    [Header("Typing Animation")]
    [Tooltip("Enable to show text letter-by-letter.")]
    public bool enableTypingEffect = false;

    [Tooltip("Typing speed in characters per second.")]
    public float typingSpeed = 40f;
}

public class Tutorial : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("Assign the TextMeshPro UI element that will display the tutorial text.")]
    public TMP_Text tutorialTextUI;

    [Header("Tutorial Sequence")]
    [Tooltip("Create your list of tutorial messages here. They will play in order.")]
    public List<TutorialStep> tutorialSteps;

    [Header("Audio Source")]
    [Tooltip("Assign an AudioSource for playing tutorial voice lines.")]
    public AudioSource audioSource;

    void Start()
    {
        // --- Validation ---
        if (tutorialTextUI == null)
        {
            Debug.LogError("Error: Tutorial Text UI is not assigned!");
            enabled = false;
            return;
        }

        if (tutorialSteps == null || tutorialSteps.Count == 0)
        {
            Debug.LogWarning("Warning: No tutorial steps defined.");
            return;
        }

        if (audioSource == null)
        {
            // Auto-attach an AudioSource if not assigned
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Start with text hidden
        tutorialTextUI.gameObject.SetActive(false);

        // Begin tutorial
        StartCoroutine(PlayTutorialSequence());
    }

    private IEnumerator PlayTutorialSequence()
    {
        Debug.Log("Tutorial sequence started.");

        foreach (TutorialStep step in tutorialSteps)
        {
            // Wait before showing this step
            yield return new WaitForSeconds(step.startDelay);

            // Show text
            tutorialTextUI.gameObject.SetActive(true);

            // Play audio if available
            if (step.dialogueAudio != null)
            {
                audioSource.clip = step.dialogueAudio;
                audioSource.Play();
            }

            // Show text with or without typing effect
            if (step.enableTypingEffect)
                yield return StartCoroutine(TypeText(step.tutorialMessage, step.typingSpeed));
            else
                tutorialTextUI.text = step.tutorialMessage;

            // Wait for duration or until audio ends (whichever is longer)
            float waitTime = step.displayDuration;
            if (step.dialogueAudio != null)
                waitTime = Mathf.Max(waitTime, step.dialogueAudio.length);

            yield return new WaitForSeconds(waitTime);

            tutorialTextUI.gameObject.SetActive(false);
        }

        Debug.Log("Tutorial sequence finished.");
    }

    private IEnumerator TypeText(string message, float typingSpeed)
    {
        tutorialTextUI.text = "";
        foreach (char c in message)
        {
            tutorialTextUI.text += c;
            yield return new WaitForSeconds(1f / typingSpeed);
        }
    }
}
