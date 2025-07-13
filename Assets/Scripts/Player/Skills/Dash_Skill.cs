using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Skill : Skill
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;

    private bool isDashing = false;

    protected override void Start()
    {
        base.Start();
        cooldown = 1f; // Set cooldown in base class
    }
    protected override void Update()
    {
        base.Update();

        if (player == null) return;

        // CHECK IF PLAYER HAS DASH SKILL USING PlayerSkills
        if (!PlayerSkills.HasDash())
        {
            return; // Dash not selected - exit
        }

        // Check for dash input
        if (Input.GetKeyDown(KeyCode.E) && !isDashing)
        {
            Debug.Log("🔥 DASH TRIGGERED BY DASH_SKILL COMPONENT!"); // Add this debug

            // Use the base class cooldown system
            if (CanUseSkill())
            {
                StartCoroutine(DashCoroutine());
            }
        }
    }

    public override void UseSkill()
    {
        // This is called by CanUseSkill() when cooldown is ready
        Debug.Log("Dash skill used!");
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;

        // Get dash direction (player's facing direction)
        float dashDirection = player.facingDirection;

        // Apply dash velocity
        player.rb.linearVelocity = new Vector2(dashSpeed * dashDirection, 0);

        // Wait for dash duration
        yield return new WaitForSeconds(dashDuration);

        // End dash
        isDashing = false;

        Debug.Log("Dash finished");
    }
}