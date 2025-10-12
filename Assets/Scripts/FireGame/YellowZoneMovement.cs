using UnityEngine;

public class YellowZoneMovement : MonoBehaviour
{
    public float moveSpeed = 2f;     // Downward speed
    public float moveUpAmount = 1f;  // How high it moves up per tap
    public float minY = -2f;
    public float maxY = 2f;

    private FishingMechanic fishingMechanic;

    void Start()
    {
        fishingMechanic = FindObjectOfType<FishingMechanic>();
        if (fishingMechanic == null)
        {
            Debug.LogError("FishingMechanic not found in scene!");
        }
    }

    void Update()
    {
        MoveDown();
    }

    private void MoveDown()
    {
        Vector2 newPosition = transform.localPosition;
        newPosition.y -= moveSpeed * Time.deltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.localPosition = newPosition;
    }

    public void OnCatchButtonTap()
    {
        Vector2 newPosition = transform.localPosition;
        newPosition.y += moveUpAmount;
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.localPosition = newPosition;
    }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fish")
        {
            if (fishingMechanic != null)
            {
                fishingMechanic?.SetFishInZone(true);
            }
        }
    }

    // Fish stays in yellow zone → Notify FishingMechanic
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fish")
        {
            if (fishingMechanic != null)
            {
                fishingMechanic?.SetFishInZone(true);
            }
        }
    }

    // Fish exits yellow zone → Notify FishingMechanic
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fish")
        {
            if (fishingMechanic != null)
            {
                fishingMechanic?.SetFishInZone(false);
            }
        }
    }

}
