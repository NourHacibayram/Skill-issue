using UnityEngine;
using UnityEngine.UI;

public class UIStatButton : MonoBehaviour
{
    [Header("Button Configuration")]
    [SerializeField] private bool isAddButton = true;
    [SerializeField] private StatsBar targetStatsBar;
    [SerializeField] private PlayerStatsManager statsManager;

    [Header("Button References")]
    [SerializeField] private Button button;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private float soundVolume = 0.7f;

    private void Start()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (statsManager == null)
            statsManager = FindFirstObjectByType<PlayerStatsManager>();
            
        // Auto-find AudioSource if not assigned
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Force enable for Speed Add Button (temporary test)
        if (gameObject.name.Contains("Speed") && isAddButton)
        {
            button.interactable = true;
            Debug.Log("Force enabled Speed Add Button");
        }

        button.onClick.AddListener(OnButtonClick);
        InvokeRepeating(nameof(UpdateButtonState), 0f, 0.1f);
    }

    private void OnButtonClick()
    {
        if (statsManager == null || targetStatsBar == null) return;

        // Play button click sound
        PlayButtonSound();

        if (isAddButton)
        {
            statsManager.AddPointToStat(targetStatsBar);
        }
        else
        {
            statsManager.RemovePointFromStat(targetStatsBar);
        }
    }
    
    private void PlayButtonSound()
    {
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound, soundVolume);
        }
    }

    private void UpdateButtonState()
    {
        if (statsManager == null || targetStatsBar == null) return;

        // Update button interactability based on game state
        if (isAddButton)
        {
            button.interactable = statsManager.CanAddPoint(targetStatsBar);
        }
        else
        {
            button.interactable = statsManager.CanRemovePoint(targetStatsBar);
        }
    }
}