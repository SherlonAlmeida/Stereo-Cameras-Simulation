using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float rotationSpeed = 2.5f;

    private Vector3 lastMousePosition;

    void Update()
    {
        // Movement
        float moveX = Input.GetAxis("Horizontal"); // A, D
        float moveZ = Input.GetAxis("Vertical");   // W, S
        float moveY = 0f;

        if (Input.GetKey(KeyCode.E)) moveY += 0.05f;
        if (Input.GetKey(KeyCode.Q)) moveY -= 0.05f;

        Vector3 move = transform.right * moveX + transform.forward * moveZ + transform.up * moveY;
        transform.position += move * moveSpeed * Time.deltaTime;

        // Mouse Drag Rotation (like Scene view in Unity)
        if (Input.GetMouseButtonDown(0)) // Left-click to start rotation
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) // Left-click to rotate
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float angleY = delta.x * rotationSpeed * Time.deltaTime;
            float angleX = -delta.y * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, angleY, Space.World);
            transform.Rotate(transform.right, angleX, Space.World);

            lastMousePosition = Input.mousePosition;
        }
    }
}
