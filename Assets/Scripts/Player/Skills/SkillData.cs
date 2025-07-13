using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("Skill Info")]
    public string skillName;
    public string description;
    public Sprite skillIcon;
    public SkillType skillType;
    
    [Header("Skill Settings")]
    public float cooldown = 2f;
    public bool isUnlocked = true; // For progression system later
}

public enum SkillType
{
    Dash,
    WallClimb,
    DoubleJump
}