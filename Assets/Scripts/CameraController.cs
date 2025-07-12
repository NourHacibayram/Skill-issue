using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target; // Player
    public float smoothTime = 0.3f;
    
    [Header("Room Bounds")]
    public Vector2 roomMin;
    public Vector2 roomMax;
    
    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    
    private void Start()
    {
        cam = GetComponent<Camera>();
        if (target == null)
            target = FindFirstObjectByType<Player>().transform;
    }
    
    // ...existing code...
    private void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 targetPosition = target.position;
        targetPosition.z = transform.position.z;
        
        // Clamp camera to room bounds
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        
        targetPosition.x = Mathf.Clamp(targetPosition.x, roomMin.x + halfWidth, roomMax.x - halfWidth);
        targetPosition.y = Mathf.Clamp(targetPosition.y, roomMin.y + halfHeight, roomMax.y - halfHeight);
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
    
    public void SetRoomBounds(Vector2 min, Vector2 max)
    {
        roomMin = min;
        roomMax = max;
    }
}