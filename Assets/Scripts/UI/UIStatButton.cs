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

    private void Start()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (statsManager == null)
            statsManager = FindFirstObjectByType<PlayerStatsManager>();

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

        if (isAddButton)
        {
            statsManager.AddPointToStat(targetStatsBar);
        }
        else
        {
            statsManager.RemovePointFromStat(targetStatsBar);
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