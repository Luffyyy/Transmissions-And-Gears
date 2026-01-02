using UnityEngine;

public class UIFollowWorld : MonoBehaviour
{
    public GameObject target;      // Gear / AR object
    public Vector3 worldOffset;   // Offset in world units
    public Vector2 screenOffset;  // Offset in pixels


    public bool setCanvasAsParent = true;

    RectTransform rect;
    Canvas canvas;
    Camera cam;

    void Awake()
    {
        if (setCanvasAsParent)
        {
            transform.SetParent(GameObject.Find("UI").transform);
        }

        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (!target || !target.activeSelf) {
            Destroy(gameObject);

            return;
        };

        Vector3 worldPos = target.transform.position + worldOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        // Hide if behind camera
        if (screenPos.z < 0)
        {
            rect.gameObject.SetActive(false);
            return;
        }

        float dist = Vector3.Distance(cam.transform.position, target.transform.position);

        rect.gameObject.SetActive(true);
        rect.position = screenPos + (Vector3)screenOffset;
        rect.localScale = Vector3.one * Mathf.Clamp(1f / dist, 0.6f, 1.2f);
    }
}
