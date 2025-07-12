/* using System.Collections;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    private float deathTimer;
    private bool isRespawning = false;

    public PlayerDeathState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // Reset flags
        deathTimer = 0f;
        isRespawning = false;
        
        // IMMEDIATELY stop all physics and movement
        StopAllMovement();
        player.ZeroVelocity();
        // Set player as busy using public method
        player.SetBusy(true);
        
        // CLEAR ALL INPUT STATES
        ClearInputStates();
        
        // Disable player input handler completely
        if (player.inputHandler != null)
        {
            player.inputHandler.enabled = false;
        }
        
        // Disable collision
        if (player.col != null)
            player.col.enabled = false;
            
        // Play death effects
        PlayDeathEffects();
        
        // Start respawn coroutine
        player.StartCoroutine(RespawnAfterDelay(0.6f));
        
        Debug.Log("Player entered death state - completely locked");
    }
    private void StopAllMovement()
    {
        // Stop Rigidbody2D completely
        if (player.rb != null)
        {
            player.rb.linearVelocity = Vector2.zero;
            player.rb.angularVelocity = 0f;
            player.rb.bodyType = RigidbodyType2D.Kinematic; // Updated API
        }

        // Stop custom velocity
        player.ZeroVelocity();
        player._frameVelocity = Vector2.zero;

        Debug.Log("All movement stopped");
    }

    public override void Update()
    {
        base.Update();

        // FORCE STOP ALL MOVEMENT AND INPUT - This is crucial
        if (player.rb != null)
        {
            player.rb.linearVelocity = Vector2.zero;
            player.rb.angularVelocity = 0f;
        }

        // Force zero custom velocity and clear inputs continuously
        player.ZeroVelocity();
        player._frameVelocity = Vector2.zero;
        
        // Continuously clear inputs during death to prevent stuck inputs
        player.ForceResetFrameInput();

        // Update death timer
        deathTimer += Time.deltaTime;

        // Handle death animation
        HandleDeathAnimation();
    }
    public override void Exit()
    {
        base.Exit();
        
        // Final input clearing
        player.ClearInputs();
        player.ResetPlayerStates();
        
        // Re-enable input handler
        if (player.inputHandler != null)
            player.inputHandler.enabled = true;
            
        // Re-enable collision
        if (player.col != null)
            player.col.enabled = true;
            
        // Make sure player isn't busy using public method
        player.SetBusy(false);
        
        Debug.Log("Player exited death state - fully restored");
    }
    private void ClearInputStates()
    {
        // Use the public method we created in Player class
        player.ClearInputs();
        player.ResetPlayerStates();
        
        // Also manually clear the frame input to be extra sure
        player.ForceResetFrameInput();

        Debug.Log("Input states cleared on death");
    }

    private void PlayDeathEffects()
    {
        // Play death particles
        if (player.landParticle != null)
            player.landParticle.Play();
    }

    private void HandleDeathAnimation()
    {
        // Optional: Fade out effect
        if (player.spriteRenderer != null)
        {
            Color color = player.spriteRenderer.color;
            color.a = Mathf.Lerp(1f, 0.3f, deathTimer / 2f);
            player.spriteRenderer.color = color;
        }
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        isRespawning = true;
        yield return new WaitForSeconds(delay);

        if (isRespawning)
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        // Find spawn point
        Vector3 spawnPosition = FindSpawnPoint();
        spawnPosition.y += 1f; // Spawn above ground

        // COMPLETELY stop all movement before teleporting
        if (player.rb != null)
        {
            player.rb.bodyType = RigidbodyType2D.Kinematic;
            player.rb.linearVelocity = Vector2.zero;
            player.rb.angularVelocity = 0f;
        }

        // Reset player position
        player.transform.position = spawnPosition;

        // CRITICAL: Clear inputs and reset states BEFORE re-enabling physics
        player.ClearInputs();
        player.ResetPlayerStates();

        // Wait a frame for everything to settle
        player.StartCoroutine(RestorePlayerAfterFrame());
    }

    private IEnumerator RestorePlayerAfterFrame()
    {
        yield return null; // Wait one frame
        yield return null; // Wait another frame for safety
        
        // Re-enable physics
        if (player.rb != null)
        {
            player.rb.bodyType = RigidbodyType2D.Dynamic;
            player.rb.linearVelocity = Vector2.zero;
        }
        
        // Reset player color/effects and ensure proper facing direction
        if (player.spriteRenderer != null)
        {
            Color color = player.spriteRenderer.color;
            color.a = 1f;
            player.spriteRenderer.color = color;
            // Force sprite to face right to match reset facing direction
            player.spriteRenderer.flipX = false;
        }
        
        // Ensure transform rotation is reset
        player.transform.rotation = Quaternion.identity;
        
        // Clear inputs multiple times for safety
        player.ClearInputs();
        player.ResetPlayerStates();
        player.ClearInputs(); // Clear again
        
        // Re-enable busy state using public method
        player.SetBusy(false);
        
        // Change back to idle state
        stateMachine.ChangeState(player.idleState);
    }
    private Vector3 FindSpawnPoint()
    {
        // Look for spawn point in scene
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");

        if (spawnPoint != null)
            return spawnPoint.transform.position;

        // Fallback position
        return new Vector3(0, 0, 0);
    }
} */