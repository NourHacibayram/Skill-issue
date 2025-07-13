using UnityEngine;

public static class PlayerSkills
{
    public static SkillType selectedSkill = SkillType.WallClimb; // Changed to WallClimb for testing
    public static bool hasSelectedSkill = true; // Changed to true for testing
    
    public static bool HasDash()
    {
        bool result = hasSelectedSkill && selectedSkill == SkillType.Dash;
        Debug.Log($"HasDash() = {result} (hasSelectedSkill: {hasSelectedSkill}, selectedSkill: {selectedSkill})");
        return result;
    }
    
    public static bool HasWallClimb()
    {
        bool result = hasSelectedSkill && selectedSkill == SkillType.WallClimb;
        Debug.Log($"HasWallClimb() = {result}");
        return result;
    }
    
    public static bool HasDoubleJump()
    {
        bool result = hasSelectedSkill && selectedSkill == SkillType.DoubleJump;
        Debug.Log($"HasDoubleJump() = {result}");
        return result;
    }
    
    public static void ResetSkills()
    {
        hasSelectedSkill = false;
        selectedSkill = SkillType.Dash;
        Debug.Log("Skills reset!");
    }
}