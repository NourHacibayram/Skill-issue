using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Level Info")]
    public int levelNumber = 1;
    public string levelName;
    
    [Header("Level Settings")]
    public Transform playerSpawnPoint;
    public Vector2 cameraMinBounds;
    public Vector2 cameraMaxBounds;
    
    [Header("Level Completion")]
    public bool isCompleted = false;
    
    private void Start()
    {
        // Use coroutine to ensure all components are initialized
        StartCoroutine(SetupLevelDelayed());
    }
    
    private System.Collections.IEnumerator SetupLevelDelayed()
    {
        // Wait one frame to ensure all Awake methods have been called
        yield return null;
        SetupLevel();
    }
    
    private void SetupLevel()
    {
        // Position player at spawn point
        if (playerSpawnPoint != null)
        {
            Player player = FindFirstObjectByType<Player>();
            if (player != null)
            {
                player.transform.position = playerSpawnPoint.position;
                
                // Try to zero velocity, but handle if rb isn't ready yet
                player.ZeroVelocity();
                
                // Alternative: directly access Rigidbody2D as fallback
                if (player.rb == null)
                {
                    Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                    if (playerRb != null)
                    {
                        playerRb.linearVelocity = Vector2.zero;
                        Debug.Log("Used fallback method to zero player velocity");
                    }
                }
            }
        }
        
        // Set camera bounds
        CameraController camera = FindFirstObjectByType<CameraController>();
        if (camera != null)
        {
            camera.SetRoomBounds(cameraMinBounds, cameraMaxBounds);
        }
        
        Debug.Log($"Level {levelNumber}: {levelName} started");
    }
    
    public void CompleteLevel()
    {
        isCompleted = true;
        GameManager.Instance.OnLevelCompleted(levelNumber);
    }
}