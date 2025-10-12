using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerRescueZone : MonoBehaviour
{
    public float radius = 3f;
    public GameObject circleVisual;

    private CircleCollider2D triggerCol;

    void Start()
    {
        triggerCol = GetComponent<CircleCollider2D>();
        triggerCol.isTrigger = true;
        triggerCol.radius = radius;
        gameObject.tag = "RescueZone"; // auto-assign tag
    }

    void Update()
    {
        // Follow mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;

        if (circleVisual != null)
            circleVisual.transform.localScale = Vector3.one * radius * 2f;
    }
}
