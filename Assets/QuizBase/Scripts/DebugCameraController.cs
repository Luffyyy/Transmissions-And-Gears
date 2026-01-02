using UnityEngine;

public class DebugCameraController : MonoBehaviour
{
    public float sensitivity = 2f;
    public float smoothing = 1.5f;

    private Vector2 mouseLook;
    private Vector2 smoothV;

    void Start()
    {
        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {
        // Get mouse input
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        // Smooth the movement
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;

        // Clamp vertical rotation so the player can't look upside down
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

        // Apply vertical rotation to the camera itself (local rotation)
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
    }
}
