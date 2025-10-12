using UnityEngine;
using UnityEngine.UI;

public class FishingGame : MonoBehaviour
{
    [Header("UI References")]
    public Image catchBarFill; // The vertical fill image (0 to 1)
    public Button catchButton; // The button the player presses

    [Header("Yellow Zone")]
    public Transform yellowZone; // The yellow zone GameObject
    public float yellowRiseAcceleration = 10f; // acceleration upwards when pressing
    public float yellowFallSpeed = 3f;         // how fast it falls down
    public float maxRiseSpeed = 6f;            // max speed cap when pressing
    public float yellowUpperLimit = 2f;
    public float yellowLowerLimit = -2f;

    [Header("Fish")]
    public Transform fish; // The fish GameObject
    public float fishMinY = -2f;
    public float fishMaxY = 2f;
    public float fishSpeedMin = 1f;
    public float fishSpeedMax = 3f;

    [Header("Catch Bar Settings")]
    public float barIncreaseRate = 0.4f;
    public float barDecreaseRate = 0.2f;
    public float startFill = 0.25f;

    private float fishSpeed;
    private int fishDirection = 1;
    private bool isPressing;
    private float barFillAmount;
    private float yellowVerticalVelocity; // current movement velocity

    private void Start()
    {
        barFillAmount = startFill;
        catchBarFill.fillAmount = barFillAmount;

        catchButton.onClick.AddListener(OnCatchButtonClick);

        fishSpeed = Random.Range(fishSpeedMin, fishSpeedMax);
        InvokeRepeating(nameof(ChangeFishDirection), 1f, 1.5f);
    }

    private void Update()
    {
        HandleYellowZoneMovement();
        HandleFishMovement();
        HandleCatchLogic();
        UpdateBar();
    }

    private void OnCatchButtonClick()
    {
        isPressing = true;
        CancelInvoke(nameof(StopPressing));
        Invoke(nameof(StopPressing), 0.05f); // short tap
    }

    private void StopPressing()
    {
        isPressing = false;
    }

    // --- Smooth Yellow Zone Movement ---
    private void HandleYellowZoneMovement()
    {
        if (isPressing)
        {
            // accelerate upward
            yellowVerticalVelocity += yellowRiseAcceleration * Time.deltaTime;
        }
        else
        {
            // apply fall speed
            yellowVerticalVelocity -= yellowFallSpeed * Time.deltaTime;
        }

        // clamp velocity to avoid insane speeds
        yellowVerticalVelocity = Mathf.Clamp(yellowVerticalVelocity, -yellowFallSpeed, maxRiseSpeed);

        // apply movement
        yellowZone.localPosition += new Vector3(0, yellowVerticalVelocity * Time.deltaTime, 0);

        // clamp within limits
        float clampedY = Mathf.Clamp(yellowZone.localPosition.y, yellowLowerLimit, yellowUpperLimit);
        yellowZone.localPosition = new Vector3(yellowZone.localPosition.x, clampedY, yellowZone.localPosition.z);

        // if touching top/bottom, stop movement to prevent jitter
        if (clampedY == yellowUpperLimit || clampedY == yellowLowerLimit)
            yellowVerticalVelocity = 0;
    }

    // --- Fish Movement ---
    private void HandleFishMovement()
    {
        fish.localPosition += new Vector3(0, fishSpeed * fishDirection * Time.deltaTime, 0);

        if (fish.localPosition.y >= fishMaxY)
        {
            fish.localPosition = new Vector3(fish.localPosition.x, fishMaxY, fish.localPosition.z);
            fishDirection = -1;
            RandomizeFishSpeed();
        }
        else if (fish.localPosition.y <= fishMinY)
        {
            fish.localPosition = new Vector3(fish.localPosition.x, fishMinY, fish.localPosition.z);
            fishDirection = 1;
            RandomizeFishSpeed();
        }
    }

    private void RandomizeFishSpeed()
    {
        fishSpeed = Random.Range(fishSpeedMin, fishSpeedMax);
    }

    private void ChangeFishDirection()
    {
        fishDirection *= Random.value > 0.5f ? 1 : -1;
    }

    // --- Catch Logic ---
    private void HandleCatchLogic()
    {
        float distance = Mathf.Abs(fish.position.y - yellowZone.position.y);
        bool isCatching = distance < 0.5f;

        if (isCatching)
            barFillAmount += barIncreaseRate * Time.deltaTime;
        else
            barFillAmount -= barDecreaseRate * Time.deltaTime;

        barFillAmount = Mathf.Clamp01(barFillAmount);
    }

    private void UpdateBar()
    {
        catchBarFill.fillAmount = barFillAmount;

        if (barFillAmount <= 0f)
        {
            Debug.Log("You Lost! The fish got away!");
        }
        else if (barFillAmount >= 1f)
        {
            Debug.Log("You Caught the Fish!");
        }
    }
}
