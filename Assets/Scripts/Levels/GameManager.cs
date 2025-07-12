using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game State")]
    public int currentLevel = 1;
    public int totalLevels = 10;
    
    [Header("Player Reference")]
    public Player player;
    
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
    
    public void LoadLevel(int levelNumber)
    {
        if (isTransitioning) return;
        
        currentLevel = levelNumber;
        string sceneName = $"Level_{levelNumber:D2}";
        StartCoroutine(TransitionToLevel(sceneName));
    }
    
    public void RestartLevel()
    {
        LoadLevel(currentLevel);
    }
    
    public void LoadNextLevel()
    {
        LoadLevel(currentLevel + 1);
    }
    
    public void OnLevelCompleted(int levelNumber)
    {
        Debug.Log($"Level {levelNumber} completed!");
        
        // Save progress
        SaveLevelProgress(levelNumber);
        
        // Load next level after delay
        if (levelNumber < totalLevels)
        {
            Invoke(nameof(LoadNextLevel), 2f);
        }
        else
        {
            Debug.Log("Game completed!");
            // Handle game completion
        }
    }
    
    private IEnumerator TransitionToLevel(string sceneName)
    {
        isTransitioning = true;
        
        // Optional: Add transition effect
        yield return StartCoroutine(FadeOut());
        
        // Load new scene
        SceneManager.LoadScene(sceneName);
        
        // Wait for scene to load
        yield return null;
        
        // Optional: Add transition effect
        yield return StartCoroutine(FadeIn());
        
        isTransitioning = false;
    }
    
    private void SaveLevelProgress(int levelNumber)
    {
        PlayerPrefs.SetInt("LastCompletedLevel", levelNumber);
        PlayerPrefs.Save();
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