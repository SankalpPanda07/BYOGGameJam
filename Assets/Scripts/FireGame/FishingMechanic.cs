using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FishingMechanic : MonoBehaviour
{
    [Header("Fish Catch Bar")]
    public Image catchBarFill;        // The UI bar that fills/unfills
    public float startFill = 0.25f;   // Starting fill amount
    public float barIncreaseRate = 0.4f;
    public float barDecreaseRate = 0.2f;

    private float barFillAmount;
    private bool fishInZone = false;

    public GameObject loose;
    public GameObject win;

    private void Start()
    {
        barFillAmount = startFill;
        catchBarFill.fillAmount = barFillAmount;
        loose.SetActive(false);
        win.SetActive(false);
    }

    private void Update()
    {
        HandleCatchLogic();
        UpdateBar();
    }

    // Only reacts to collider-based detection
    private void HandleCatchLogic()
    {
        if (fishInZone)
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
            loose.SetActive(true);
            Debug.Log("❌ You Lost! The fish got away!");
        }
        else if (barFillAmount >= 1f)
        {
            Debug.Log("✅ You Caught the Fish!");
            //SceneManager.LoadScene("Iron");
            win.SetActive(true);
        }
    }

    // Called by YellowZoneMovement collider events
    public void SetFishInZone(bool value)
    {
        fishInZone = value;
    }
}
