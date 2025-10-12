using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private float minY = -2f;
    [SerializeField] private float maxY = 2f;
    [SerializeField] private float speed = 2f;

    private float targetY;

    void Start()
    {
        SetNewTarget();
    }

    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, targetY), speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.y - targetY) < 0.1f)
        {
            SetNewTarget();
        }
    }

    private void SetNewTarget()
    {
        targetY = Random.Range(minY, maxY);
    }
}
