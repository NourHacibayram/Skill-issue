using System.Collections;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public enum DeathType
    {
        Spike,
        Void
    }

    private DeathType deathType;
    private float respawnDelay = 2f;
    private bool hasRespawned = false;
    private float originalGravityScale;

    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // Store original gravity scale and disable gravity
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0f;
        
        // Stop all movement
        player.ZeroVelocity();
        
        // Disable player input temporarily
        player.StartCoroutine(player.BusyFor(respawnDelay));
        
        // Play death animation based on death type
        PlayDeathAnimation();
        
        // Play death sound
        player.PlayDeathSound();
        
        // Start respawn countdown
        hasRespawned = false;
        player.StartCoroutine(RespawnAfterDelay());
    }

    public override void Update()
    {
        base.Update();
        
        // Keep player completely stationary during death state
        player.ZeroVelocity();
        
        // Ensure gravity stays disabled
        if (rb.gravityScale != 0f)
        {
            rb.gravityScale = 0f;
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        // Restore original gravity when leaving dead state
        rb.gravityScale = originalGravityScale;
        
        // Reset any death-related flags
        hasRespawned = true;
    }

    public void SetDeathType(DeathType type)
    {
        deathType = type;
    }

    private void PlayDeathAnimation()
    {
        switch (deathType)
        {
            case DeathType.Spike:
                // Play shock/electrocution animation
                player.anim.SetTrigger("ShockLight");
                break;
            case DeathType.Void:
                // Play falling animation or keep current animation
                player.anim.SetTrigger("Death");
                break;
        }
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        
        if (!hasRespawned)
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        // Find the current level and spawn point
        Level currentLevel = Object.FindFirstObjectByType<Level>();
        
        if (currentLevel != null && currentLevel.playerSpawnPoint != null)
        {
            // Teleport player to spawn point
            player.transform.position = currentLevel.playerSpawnPoint.position;
            
            // Add some upward velocity if died from void (thrown back effect)
            if (deathType == DeathType.Void)
            {
                player.SetVelocity(0, player.jumpForce * 0.8f); // Slight upward throw
            }
            else
            {
                player.ZeroVelocity();
            }
            
            // Transition back to idle state
            stateMachine.ChangeState(player.idleState);
        }
        else
        {
            Debug.LogWarning("No spawn point found! Cannot respawn player.");
            // Fallback: just transition to idle state
            stateMachine.ChangeState(player.idleState);
        }
    }
}
