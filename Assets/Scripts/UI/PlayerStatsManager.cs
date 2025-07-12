using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour
{
    [Header("Total Points System")]
    [SerializeField] private int totalAvailablePoints = 5;
    [SerializeField] private int currentUsedPoints = 0;

    [Header("Stat Bars")]
    [SerializeField] private StatsBar jumpForceBar;
    [SerializeField] private StatsBar movementSpeedBar;
    [SerializeField] private StatsBar dashForceBar; // Optional: for dash ability

    [Header("UI Elements")]
    [SerializeField] private TMPro.TextMeshProUGUI remainingPointsText; // Change this line

    [Header("Player Reference")]
    [SerializeField] private Player player;

    // Starting values for each stat
    private int startingJumpPoints = 1;    // Changed from 2 to 1
    private int startingSpeedPoints = 1;   // Changed from 3 to 1  
    private int startingDashPoints = 1;    // Changed from 2 to 1

    private void Start()
    {
        Debug.Log("=== PlayerStatsManager Start Debug ===");

        // Check all stat bars
        Debug.Log($"jumpForceBar is null: {jumpForceBar == null}");
        Debug.Log($"movementSpeedBar is null: {movementSpeedBar == null}");
        Debug.Log($"dashForceBar is null: {dashForceBar == null}");

        if (player == null)
        {
            player = FindFirstObjectByType<Player>();
            Debug.Log($"Player found: {player != null}");
        }

        // Only require jump and speed bars (dash is optional)
        if (jumpForceBar != null && movementSpeedBar != null)
        {
            InitializeStats();
            UpdatePlayerStats();
            UpdateUI();
            Debug.Log("Stats initialized successfully!");
        }
        else
        {
            Debug.LogError("Jump or Speed stat bar is not assigned in Inspector!");
        }
    }

    private void InitializeStats()
    {
        Debug.Log("Before SetCurrentValue:");
        Debug.Log($"Jump bar current value: {jumpForceBar.GetCurrentValue()}");
        Debug.Log($"Speed bar current value: {movementSpeedBar.GetCurrentValue()}");

        // Set starting values (all stats start at 1)
        jumpForceBar.SetCurrentValue(startingJumpPoints);
        movementSpeedBar.SetCurrentValue(startingSpeedPoints);

        Debug.Log("After SetCurrentValue:");
        Debug.Log($"Jump bar current value: {jumpForceBar.GetCurrentValue()}");
        Debug.Log($"Speed bar current value: {movementSpeedBar.GetCurrentValue()}");

        // Only set dash if it exists
        if (dashForceBar != null)
            dashForceBar.SetCurrentValue(startingDashPoints);

        // Calculate used points (only count points above minimum)
        RecalculateUsedPoints();
    }


    public bool CanAddPoint(StatsBar targetBar)
    {
        // Check if we have points available and target isn't at max
        return currentUsedPoints < totalAvailablePoints &&
               targetBar.GetCurrentValue() < targetBar.GetMaxValue();
    }

    public bool CanRemovePoint(StatsBar targetBar)
    {
        // Check if target has points to remove
        return targetBar.GetCurrentValue() > targetBar.GetMinValue();
    }

    public void AddPointToStat(StatsBar targetBar)
    {
        if (CanAddPoint(targetBar))
        {
            targetBar.AddPoint();
            RecalculateUsedPoints();
            UpdatePlayerStats();
            UpdateUI();
        }
    }

    public void RemovePointFromStat(StatsBar targetBar)
    {
        if (CanRemovePoint(targetBar))
        {
            targetBar.RemovePoint();
            RecalculateUsedPoints();
            UpdatePlayerStats();
            UpdateUI();
        }
    }

    private void RecalculateUsedPoints()
    {
        // Only count points above the minimum (1) for each stat
        int jumpExtraPoints = jumpForceBar.GetCurrentValue() - 1;
        int speedExtraPoints = movementSpeedBar.GetCurrentValue() - 1;
        int dashExtraPoints = dashForceBar != null ? dashForceBar.GetCurrentValue() - 1 : 0;

        currentUsedPoints = jumpExtraPoints + speedExtraPoints + dashExtraPoints;
    }
    private void UpdatePlayerStats()
    {
        if (player == null) return;

        // Base stats
        float baseJumpForce = 8f;
        float baseMovementSpeed = 5f;

        // Get the actual point values (1-5)
        int jumpPoints = jumpForceBar.GetCurrentValue();
        int speedPoints = movementSpeedBar.GetCurrentValue();

        // Calculate multipliers based on points (each point = 20% of base)
        // Point 1 = base stats, Point 2 = +20%, Point 3 = +40%, etc.
        float jumpMultiplier = jumpPoints * 0.2f; // 1=0.2, 2=0.4, 3=0.6, etc.
        float speedMultiplier = speedPoints * 0.2f;

        // Apply to player
        player.jumpForce = baseJumpForce + (baseJumpForce * jumpMultiplier);
        player.moveSpeed = baseMovementSpeed + (baseMovementSpeed * speedMultiplier);

        Debug.Log($"Stats Updated - Points: Jump:{jumpPoints}, Speed:{speedPoints} | Values: Jump: {player.jumpForce:F1}, Speed: {player.moveSpeed:F1}");
    }

    private void UpdateUI()
    {
        if (remainingPointsText != null)
        {
            int remaining = totalAvailablePoints - currentUsedPoints;
            remainingPointsText.text = $"{remaining}/{totalAvailablePoints}";
        }
    }

    public int GetRemainingPoints()
    {
        return totalAvailablePoints - currentUsedPoints;
    }

    // Reset to starting configuration
    public void ResetStats()
    {
        InitializeStats();
        UpdatePlayerStats();
        UpdateUI();
    }
}