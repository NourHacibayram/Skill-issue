using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites = new Sprite[5];
    [SerializeField] private Image barImage; // The UI Image component that displays the sprite
    
    [Header("Current Stats")]
    [SerializeField] private int currentValue = 0;
    [SerializeField] private int maxValue = 4;
    [SerializeField] private int minValue = 0;
    
    private void Start()
    {
        // Initialize with current value
        UpdateSprite();
    }
    
    public void AddPoint()
    {
        if (currentValue < maxValue)
        {
            currentValue++;
            UpdateSprite();
        }
    }
    
    public void RemovePoint()
    {
        if (currentValue > minValue)
        {
            currentValue--;
            UpdateSprite();
        }
    }
    
    private void UpdateSprite()
    {
        if (barImage != null && sprites != null && currentValue < sprites.Length)
        {
            barImage.sprite = sprites[currentValue];
        }
    }
    
    // Getter for current value
    public int GetCurrentValue()
    {
        return currentValue;
    }
    
    // Setter for current value (with bounds checking)
    public void SetCurrentValue(int value)
    {
        currentValue = Mathf.Clamp(value, minValue, maxValue);
        UpdateSprite();
    }
}
