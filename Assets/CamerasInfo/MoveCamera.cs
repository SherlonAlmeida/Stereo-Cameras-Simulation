using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float rotationSpeed = 2.5f;

    private Vector3 lastMousePosition;

    void Update()
    {
        // Movimento
        float moveX = Input.GetAxis("Horizontal"); // A, D
        float moveZ = Input.GetAxis("Vertical");   // W, S
        float moveY = 0f;

        if (Input.GetKey(KeyCode.E)) moveY += 0.05f;
        if (Input.GetKey(KeyCode.Q)) moveY -= 0.05f;

        Vector3 move = transform.right * moveX + transform.forward * moveZ + transform.up * moveY;
        transform.position += move * moveSpeed * Time.deltaTime;

        // Rotação horizontal via mouse
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float angleY = delta.x * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, angleY, Space.World); // somente horizontal

            lastMousePosition = Input.mousePosition;
        }
    }
}
