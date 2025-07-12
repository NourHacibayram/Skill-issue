using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites = new Sprite[6]; // 1-5 sprites (index 0 unused)
    [SerializeField] private Image barImage;

    [Header("Current Stats")]
    [SerializeField] private int currentValue = 1;  // Changed from 0 to 1
    [SerializeField] private int maxValue = 5;
    [SerializeField] private int minValue = 1;      // Changed from 0 to 1

    private void Start()
    {
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
    if (barImage == null || sprites == null || sprites.Length == 0)
        return;
    
    // Use currentValue - 1 as index (value 1->index 0, value 2->index 1, etc.)
    int spriteIndex = currentValue - 1;
    
    if (spriteIndex >= 0 && spriteIndex < sprites.Length)
    {
        Debug.Log($"Using sprite index {spriteIndex} for stat value {currentValue}");
        barImage.sprite = sprites[spriteIndex];
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