using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMenStart : MonoBehaviour
{
    [Header("References")]
    public GameObject player;      // Assign your Player GameObject here
    public GameObject startCanvas; // Assign your Canvas here
    public float startDelay = 3f;  // Delay in seconds

    private void Start()
    {
        startCanvas.SetActive(true);
        player.SetActive(false);
        // Start the level start routine
        StartCoroutine(StartLevelRoutine());
    }

    private IEnumerator StartLevelRoutine()
    {
        // Disable player and enable canvas
        if (player != null) player.SetActive(false);
        if (startCanvas != null) startCanvas.SetActive(true);

        // Wait for the delay
        yield return new WaitForSeconds(startDelay);

        // Enable player and disable canvas
        if (player != null) player.SetActive(true);
        if (startCanvas != null) startCanvas.SetActive(false);
    }
}
