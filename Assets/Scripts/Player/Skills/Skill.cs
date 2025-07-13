using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;
    protected Player player;


    protected virtual void Start()
    {
        // Find player safely
        player = GetComponent<Player>();

        // If not found on same GameObject, try finding in scene
        if (player == null)
        {
            player = FindFirstObjectByType<Player>();
        }

        // Check if player was found
        if (player == null)
        {
            Debug.LogError($"Player not found for skill: {GetType().Name}");
            return;
        }

        // Initialize skill after player is found
        InitializeSkill();
    }


    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer <= 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        Debug.Log("Skill is on cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        Debug.Log("Skill used");
    }
    protected virtual void InitializeSkill()
    {
        // Override this in child classes
    }
}