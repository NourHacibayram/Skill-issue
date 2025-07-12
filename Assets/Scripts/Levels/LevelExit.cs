using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [Header("Exit Settings")]
    public int nextLevelNumber;
    public bool requiresKey = false;
    public bool requiresPuzzleSolved = false;
    
    [Header("Visual Feedback")]
    public GameObject exitEffect;
    public AudioClip exitSound;
    
    private bool canExit = true;
    
    private void Start()
    {
        // Check if exit should be available
        UpdateExitAvailability();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canExit)
        {
            if (CanPlayerExit())
            {
                ExitLevel();
            }
            else
            {
                ShowExitRequirements();
            }
        }
    }
    
    private bool CanPlayerExit()
    {
        if (requiresKey && !HasKey()) return false;
        if (requiresPuzzleSolved && !IsPuzzleSolved()) return false;
        return true;
    }
    
    private void ExitLevel()
    {
        // Complete current level
        Level currentLevel = FindFirstObjectByType<Level>();
        if (currentLevel != null)
        {
            currentLevel.CompleteLevel();
        }
        
        // Load next level
        GameManager.Instance.LoadLevel(nextLevelNumber);
    }
    
    private void UpdateExitAvailability()
    {
        canExit = CanPlayerExit();
        
        // Update visual state
        if (exitEffect != null)
        {
            exitEffect.SetActive(canExit);
        }
    }
    
    private void ShowExitRequirements()
    {
        if (requiresKey && !HasKey())
        {
            Debug.Log("Need a key to exit!");
        }
        if (requiresPuzzleSolved && !IsPuzzleSolved())
        {
            Debug.Log("Solve the puzzle to exit!");
        }
    }
    
    private bool HasKey() { /* Implementation */ return true; }
    private bool IsPuzzleSolved() { /* Implementation */ return true; }
}