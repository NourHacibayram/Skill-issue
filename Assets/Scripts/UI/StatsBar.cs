using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites = new Sprite[5]; // 1-5 sprites (index 0 unused)
    [SerializeField] private Image barImage;

    [Header("Current Stats")]
    [SerializeField] private int currentValue = 1;  // Changed from 0 to 1
    [SerializeField] private int maxValue = 5;
    [SerializeField] private int minValue = 1;      // Changed from 0 to 1

    private void Awake()
    {
        Debug.Log($"StatsBar Awake START - currentValue: {currentValue}, min: {minValue}, max: {maxValue}");
        
        // Force set to minValue if somehow it's wrong
        if (currentValue <= 0)
        {
            Debug.LogError($"CRITICAL: currentValue is {currentValue} in Awake! This should never happen. Forcing to minValue.");
            currentValue = minValue;
        }
        
        // Ensure currentValue is within valid bounds
        if (currentValue < minValue)
        {
            Debug.LogWarning($"currentValue {currentValue} was below minValue {minValue}. Setting to minValue.");
            currentValue = minValue;
        }
        if (currentValue > maxValue)
        {
            Debug.LogWarning($"currentValue {currentValue} was above maxValue {maxValue}. Setting to maxValue.");
            currentValue = maxValue;
        }
        
        Debug.Log($"StatsBar Awake END - currentValue: {currentValue}, min: {minValue}, max: {maxValue}");
    }

    private void Start()
    {
        Debug.Log($"StatsBar Start - currentValue: {currentValue}");
        UpdateSprite();
    }

    public void AddPoint()
    {
        if (currentValue < maxValue)
        {
            currentValue++;
            UpdateSprite(); // Make sure this is called
            Debug.Log($"Added point. Current value: {currentValue}");
        }
    }

    public void RemovePoint()
    {
        if (currentValue > minValue)
        {
            currentValue--;
            UpdateSprite(); // Make sure this is called
            Debug.Log($"Removed point. Current value: {currentValue}");
        }
    }

    private void UpdateSprite()
    {
        Debug.Log($"UpdateSprite called - currentValue: {currentValue}, minValue: {minValue}, maxValue: {maxValue}");
        
        // EMERGENCY FIX: If currentValue is somehow 0 or invalid, force it to minValue
        if (currentValue <= 0)
        {
            Debug.LogError($"EMERGENCY FIX: currentValue was {currentValue}, forcing to minValue {minValue}");
            currentValue = minValue;
        }
        
        if (barImage == null)
        {
            Debug.LogError("barImage is null!");
            return;
        }
        
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError("sprites array is null or empty!");
            return;
        }
        
        // Ensure currentValue is within bounds
        if (currentValue < minValue)
        {
            Debug.LogWarning($"currentValue {currentValue} is below minValue {minValue}. Clamping.");
            currentValue = minValue;
        }
        
        // Use currentValue - 1 as index (value 1->index 0, value 2->index 1, etc.)
        int spriteIndex = currentValue - 1;
        
        Debug.Log($"Calculated sprite index: {spriteIndex} (currentValue: {currentValue})");
        
        if (spriteIndex >= 0 && spriteIndex < sprites.Length)
        {
            if (sprites[spriteIndex] != null)
            {
                Debug.Log($"Setting sprite at index {spriteIndex} for stat value {currentValue}");
                barImage.sprite = sprites[spriteIndex];
            }
            else
            {
                Debug.LogError($"Sprite at index {spriteIndex} is null!");
            }
        }
        else
        {
            Debug.LogError($"Sprite index {spriteIndex} out of range for sprites array (length: {sprites.Length})");
        }
    }
    public int GetCurrentValue() => currentValue;
    public int GetMaxValue() => maxValue;
    public int GetMinValue() => minValue;

    public void SetCurrentValue(int value)
    {
        currentValue = Mathf.Clamp(value, minValue, maxValue);
        UpdateSprite(); // Make sure this is called
        Debug.Log($"Set value to: {currentValue}");
    }

}