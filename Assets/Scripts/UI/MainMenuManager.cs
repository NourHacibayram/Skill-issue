using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private UnityEngine.UI.Button playButton;
    [SerializeField] private UnityEngine.UI.Button settingsButton;
    [SerializeField] private UnityEngine.UI.Button quitButton;
    
    [Header("Scene Management")]
    [SerializeField] private string cutsceneSceneName = "Scenes/Intro"; // Your Intro scene
    [SerializeField] private string firstLevelName = "Scenes/Levels/Level_01"; // Your first level
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip menuClickSound;
    [SerializeField] private float soundVolume = 0.7f;
    
    private void Start()
    {
        // Auto-find AudioSource if not assigned
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
            
        // Add button listeners
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OpenSettings);
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }
    
    private void PlayMenuSound()
    {
        if (menuClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(menuClickSound, soundVolume);
        }
    }
    
    public void PlayGame()
    {
        PlayMenuSound();
        Debug.Log("Starting game... Loading intro cutscene");
        
        // Load your Intro scene (index 1)
        SceneManager.LoadScene("Scenes/Intro");
    }
    
    public void PlayGameSkipIntro()
    {
        PlayMenuSound();
        Debug.Log("Starting game... Skipping intro, going to Level 1");
        
        // Skip intro and go directly to Level_01 (index 2)
        SceneManager.LoadScene("Scenes/Levels/Level_01");
    }
    
    public void OpenSettings()
    {
        PlayMenuSound();
        Debug.Log("Opening settings...");
        // TODO: Implement settings menu
    }
    
    public void QuitGame()
    {
        PlayMenuSound();
        Debug.Log("Quitting game...");
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    // Utility method for loading any level by name
    public void LoadLevel(string levelName)
    {
        PlayMenuSound();
        Debug.Log($"Loading level: {levelName}");
        SceneManager.LoadScene(levelName);
    }
    
    // Load specific level by index (for testing)
    public void LoadLevelByIndex(int levelIndex)
    {
        PlayMenuSound();
        Debug.Log($"Loading level index: {levelIndex}");
        SceneManager.LoadScene(levelIndex);
    }
}