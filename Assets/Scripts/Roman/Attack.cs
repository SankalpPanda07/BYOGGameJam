using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Sword Settings")]
    public Transform sword;             // Reference to the sword GameObject
    public float swingSpeed = 600f;     // Rotation speed
    public float swingAngle = 90f;      // Total swing angle
    public float cooldownTime = 0.5f;   // Delay between swings

    private bool isSwinging = false;
    private bool canSwing = true;
    private float startAngle;
    private float targetAngle;
    private float currentSwingTime;

    void Update()
    {
        // Left Mouse Button to swing
        if (Input.GetMouseButtonDown(0) && canSwing)
        {
            StartSwing();
        }

        if (isSwinging)
        {
            RotateSword();
        }
    }

    void StartSwing()
    {
        isSwinging = true;
        canSwing = false;
        currentSwingTime = 0f;

        // Start and end rotation angles (pivot at handle)
        startAngle = -swingAngle / 2f;
        targetAngle = swingAngle / 2f;

        sword.localRotation = Quaternion.Euler(0, 0, startAngle);
    }

    void RotateSword()
    {
        currentSwingTime += Time.deltaTime * swingSpeed / swingAngle;
        float t = currentSwingTime;
        sword.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(startAngle, targetAngle, t));

        // When done swinging
        if (t >= 1f)
        {
            isSwinging = false;
            sword.localRotation = Quaternion.identity; // Reset rotation
            Invoke(nameof(ResetSwing), cooldownTime);
        }
    }

    void ResetSwing()
    {
        canSwing = true;
    }
}
