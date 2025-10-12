using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The speed at which the player moves towards the mouse.")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector2 mouseWorldPosition;

    void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main; 
    }

    void Update()
    {

        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
    void FixedUpdate()
    {

        RotateTowardsMouse();
        MoveTowardsMouse();
    }

    private void RotateTowardsMouse()
    {
        Vector2 lookDirection = mouseWorldPosition - rb.position;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        angle -= 90f;

        rb.rotation = angle;
    }

    private void MoveTowardsMouse()
    {
        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            mouseWorldPosition,
            moveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);
    }
}
