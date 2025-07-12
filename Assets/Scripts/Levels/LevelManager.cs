using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Current Level Info")]
    public int currentLevel = 1;
    public int currentRoom = 1;

    [Header("Player Reference")]
    public Player player;

    private Vector2 pendingSpawnPosition;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadRoom(string sceneName, Vector2 spawnPosition)
    {
        if (isTransitioning) return;

        StartCoroutine(TransitionToRoom(sceneName, spawnPosition));
    }

    private IEnumerator TransitionToRoom(string sceneName, Vector2 spawnPosition)
    {
        isTransitioning = true;
        pendingSpawnPosition = spawnPosition;

        // Optional: Add transition effect (fade out)
        yield return StartCoroutine(FadeOut());

        // Load new scene
        SceneManager.LoadScene(sceneName);

        // Wait for scene to load
        yield return null;

        // Position player at spawn point
        PositionPlayer();

        // Optional: Add transition effect (fade in)
        yield return StartCoroutine(FadeIn());

        isTransitioning = false;
    }

    private void PositionPlayer()
    {
        if (player == null)
            player = FindFirstObjectByType<Player>();

        if (player != null)
        {
            player.transform.position = pendingSpawnPosition;
            // Reset player state if needed
            player.ZeroVelocity();
        }
    }

    private IEnumerator FadeOut()
    {
        // Implement fade transition
        yield return new WaitForSeconds(0.3f);
    }

    private IEnumerator FadeIn()
    {
        // Implement fade transition
        yield return new WaitForSeconds(0.3f);
    }
}