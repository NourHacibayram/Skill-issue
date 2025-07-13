using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSelectionButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image skillIcon;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillDescriptionText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Button button;
    
    [Header("Visual States")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;
    
    private SkillData skillData;
    private SkillSelectionManager manager;
    private bool isSelected = false;
    
    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
            
        button.onClick.AddListener(OnButtonClick);
    }
    
    public void Setup(SkillData skill, SkillSelectionManager selectionManager)
    {
        skillData = skill;
        manager = selectionManager;
        
        // Update UI
        if (skillIcon != null)
            skillIcon.sprite = skill.skillIcon;
            
        if (skillNameText != null)
            skillNameText.text = skill.skillName;
            
        if (skillDescriptionText != null)
            skillDescriptionText.text = skill.description;
            
        // Set interactable based on unlock status
        button.interactable = skill.isUnlocked;
        
        SetSelected(false);
    }
    
    private void OnButtonClick()
    {
        if (skillData != null && manager != null)
        {
            manager.SelectSkill(skillData);
        }
    }
    
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        
        if (backgroundImage != null)
        {
            backgroundImage.color = selected ? selectedColor : normalColor;
        }
    }
    
    public SkillData GetSkillData()
    {
        return skillData;
    }
}