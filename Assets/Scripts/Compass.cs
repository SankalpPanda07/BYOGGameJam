using UnityEngine;
using TMPro; 
public class Compass : MonoBehaviour
{
    [Header("References")]
    public Transform player;          // Player's position
    public Transform target;          // Target object to track
    public TextMeshProUGUI distanceText; // Text UI for displaying distance

    [Header("Settings")]
    public bool showInWholeMeters = true; // Round off the meter display

    void Update()
    {
        if (player == null || target == null || distanceText == null)
            return;

        // Calculate distance in world space
        float distance = Vector2.Distance(player.position, target.position);

        // Optionally round off
        if (showInWholeMeters)
            distance = Mathf.Round(distance);

        // Update text (example: "15m")
        distanceText.text = distance.ToString("0") + "m";
    }
}