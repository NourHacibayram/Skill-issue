using UnityEngine;
using UnityEngine.UI;

public class UIStatButton : MonoBehaviour
{
    [Header("Button Configuration")]
    [SerializeField] private bool isAddButton = true; // True for add, False for remove
    [SerializeField] private StatsBar targetStatsBar;
    
    [Header("Button References")]
    [SerializeField] private Button button;
    
    private void Start()
    {
        Debug.Log($"UIStatButton Start - IsAddButton: {isAddButton}");
        
        // Get button component if not assigned
        if (button == null)
        {
            button = GetComponent<Button>();
            Debug.Log(button != null ? "Button component found automatically" : "No Button component found on this GameObject!");
        }
        else
        {
            Debug.Log("Button component was already assigned");
        }
        
        // Add listener to button
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
            Debug.Log("Button click listener added successfully");
        }
        
        // Auto-find StatsBar if not assigned
        if (targetStatsBar == null)
        {
            targetStatsBar = FindObjectOfType<StatsBar>();
            Debug.Log(targetStatsBar != null ? "StatsBar found automatically in scene" : "No StatsBar found in scene!");
        }
        else
        {
            Debug.Log("StatsBar was already assigned");
        }
    }
    
    private void OnButtonClick()
    {
        Debug.Log($"Button clicked! IsAddButton: {isAddButton}");
        
        if (targetStatsBar == null) 
        {
            Debug.LogError("TargetStatsBar is null! Make sure to assign it in the inspector or ensure a StatsBar exists in the scene.");
            return;
        }
        
        if (isAddButton)
        {
            Debug.Log("Adding point to stats bar");
            targetStatsBar.AddPoint();
        }
        else
        {
            Debug.Log("Removing point from stats bar");
            targetStatsBar.RemovePoint();
        }
        
        Debug.Log($"Current stats bar value: {targetStatsBar.GetCurrentValue()}");
    }
    
    // Public method to set the target stats bar
    public void SetTargetStatsBar(StatsBar statsBar)
    {
        targetStatsBar = statsBar;
    }
    
    // Public method to set button type
    public void SetButtonType(bool addButton)
    {
        isAddButton = addButton;
    }
}
