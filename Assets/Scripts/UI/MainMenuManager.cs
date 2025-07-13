using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private UnityEngine.UI.Button playButton;
    [SerializeField] private UnityEngine.UI.Button settingsButton;
    [SerializeField] private UnityEngine.UI.Button quitButton;
    
    
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
        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(OpenSettings);
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
        Debug.Log("Starting game...");
        SceneManager.LoadScene(1);
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
        Application.Quit();
    }
}