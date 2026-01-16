using UnityEngine;

public class GearTouchControls : MonoBehaviour
{
    public float rotationSpeed = 0.001f;
    public float zoomSpeed = 0.001f;
    public float minScale = 0.1f;
    public float maxScale = 0.75f;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float rotation = -touch.deltaPosition.x * rotationSpeed;
                transform.Rotate(0, rotation, 0, Space.World);
            }
        }

        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 t0Prev = t0.position - t0.deltaPosition;
            Vector2 t1Prev = t1.position - t1.deltaPosition;

            float prevDistance = Vector2.Distance(t0Prev, t1Prev);
            float currentDistance = Vector2.Distance(t0.position, t1.position);

            float delta = currentDistance - prevDistance;

            float scaleFactor = delta * zoomSpeed;

            Vector3 newScale = transform.localScale + Vector3.one * scaleFactor;
            newScale = Vector3.ClampMagnitude(newScale, maxScale);

            // Manual clamp (safer)
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

            transform.localScale = newScale;
        }

        if (Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse Y") * 5f;
            float rotY = -Input.GetAxis("Mouse X") * 5f;

            transform.Rotate(rotX, rotY, 0, Space.World);
        }
    }
}
