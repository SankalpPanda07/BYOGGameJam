using UnityEngine;
using UnityEngine.Events; // optional for UI or events

public class CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 10f;        // seconds
    public bool autoStart = true;        // start on Awake/Start
    public string messageOnEnd = "Time's up!";

    [Header("Events")]
    public UnityEvent onTimerEnd;        // can be used for UI updates or other actions

    private float currentTime;
    private bool running = false;

    void Start()
    {
        currentTime = startTime;

        if (autoStart)
            StartTimer();
    }

    void Update()
    {
        if (!running) return;

        currentTime -= Time.deltaTime;

        // Optional: update UI here, e.g., via onTimerEnd
        // Example: Debug.Log(currentTime);

        if (currentTime <= 0f)
        {
            running = false;
            currentTime = 0f;
            TimerEnded();
        }
    }

    public void StartTimer()
    {
        currentTime = startTime;
        running = true;
    }

    public void StopTimer()
    {
        running = false;
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        running = false;
    }

    private void TimerEnded()
    {
        Debug.Log(messageOnEnd);

        // Trigger event for UI or other systems
        if (onTimerEnd != null)
            onTimerEnd.Invoke();
    }

    // Optional: get remaining time
    public float GetTimeLeft()
    {
        return currentTime;
    }
}
