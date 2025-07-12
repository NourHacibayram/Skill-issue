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
                player.ZeroVelocity();
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