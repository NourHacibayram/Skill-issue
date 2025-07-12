using UnityEngine;
using UnityEngine.UI;

public class UIStatsManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private StatsBar statsBar;
    [SerializeField] private Button addButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private Text valueText; // Optional: Display current value as text
    
    [Header("Audio (Optional)")]
    [SerializeField] private AudioClip addSound;
    [SerializeField] private AudioClip removeSound;
    [SerializeField] private AudioSource audioSource;
    
    private void Start()
    {
        // Setup button listeners
        if (addButton != null)
        {
            addButton.onClick.AddListener(OnAddButtonClick);
        }
        
        if (removeButton != null)
        {
            removeButton.onClick.AddListener(OnRemoveButtonClick);
        }
        
        // Update initial display
        UpdateDisplay();
    }
    
    private void OnAddButtonClick()
    {
        if (statsBar != null)
        {
            statsBar.AddPoint();
            PlaySound(addSound);
            UpdateDisplay();
        }
    }
    
    private void OnRemoveButtonClick()
    {
        if (statsBar != null)
        {
            statsBar.RemovePoint();
            PlaySound(removeSound);
            UpdateDisplay();
        }
    }
    
    private void UpdateDisplay()
    {
        if (valueText != null && statsBar != null)
        {
            valueText.text = statsBar.GetCurrentValue().ToString();
        }
    }
    
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    
    // Public methods for external access
    public void ResetStats()
    {
        if (statsBar != null)
        {
            statsBar.SetCurrentValue(0);
            UpdateDisplay();
        }
    }
    
    public void MaxStats()
    {
        if (statsBar != null)
        {
            statsBar.SetCurrentValue(4);
            UpdateDisplay();
        }
    }
}
