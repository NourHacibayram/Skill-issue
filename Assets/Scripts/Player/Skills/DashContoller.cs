using UnityEngine;
using System.Collections;

public static class DashController
{
    private static float dashSpeed = 20f;
    private static float dashDuration = 0.2f;
    private static float dashCooldown = 1f;
    private static float cooldownTimer = 0f;
    private static bool isDashing = false;

    public static void Update()
    {
        // Update cooldown
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public static bool CanDash()
    {
        // CHECK IF DASH IS THE SELECTED SKILL!
        return PlayerSkills.HasDash() && cooldownTimer <= 0 && !isDashing;
    }

    public static void TryDash(Player player)
    {
        if (!CanDash()) 
        {
            // Debug why dash was blocked
            if (!PlayerSkills.HasDash())
                Debug.Log("Dash blocked: Not selected skill");
            else if (cooldownTimer > 0)
                Debug.Log($"Dash blocked: Cooldown {cooldownTimer:F1}s remaining");
            else if (isDashing)
                Debug.Log("Dash blocked: Already dashing");
            return;
        }

        // Start dash
        isDashing = true;
        cooldownTimer = dashCooldown;

        // Apply dash velocity
        float dashDirection = player.facingDirection;
        player.rb.linearVelocity = new Vector2(dashSpeed * dashDirection, 0);

        // Start coroutine to end dash
        player.StartCoroutine(EndDashAfterDuration());

        Debug.Log("Dash activated via DashController!");
    }

    private static IEnumerator EndDashAfterDuration()
    {
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        Debug.Log("Dash finished");
    }
}