using UnityEngine;

public class GearTouchControls : MonoBehaviour
{
    public float rotationSpeed = 2.5f;
    public float zoomSpeed = 0.01f;
    public float minScale = 0.1f;
    public float maxScale = 0.75f;

    private float startT;

    void Start()
    {
        startT = Time.time + 1;
    }

    void Update()
    {
        if (startT != 0 && startT >= Time.time) // Prevent sudden move when moving slides
        {
            startT = 0;
            return;
        }

        Touch t0 = Input.GetTouch(0);
        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(1);

            Vector2 t0Prev = t0.position - t0.deltaPosition;
            Vector2 t1Prev = t1.position - t1.deltaPosition;

            float prevDistance = Vector2.Distance(t0Prev, t1Prev);
            float currentDistance = Vector2.Distance(t0.position, t1.position);

            float delta = currentDistance - prevDistance;

            float scaleFactor = delta * zoomSpeed * Time.deltaTime;

            Vector3 newScale = transform.localScale + Vector3.one * scaleFactor;
            newScale = Vector3.ClampMagnitude(newScale, maxScale);

            // Manual clamp (safer)
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

            transform.localScale = newScale;
        } else if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // Rotate around the Y-axis based on horizontal finger movement
                transform.Rotate(Vector3.up, -Mathf.Sign(touch.deltaPosition.x) * rotationSpeed, Space.World);

                // Rotate around the X-axis based on vertical finger movement
                transform.Rotate(Vector3.right, Mathf.Sign(touch.deltaPosition.y) * rotationSpeed, Space.World);
            }
        }
    }
}
