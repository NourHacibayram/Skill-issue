using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbIdleState : PlayerState
{
    private float originalGravityScale; // Store the original gravity scale
    
    public PlayerWallClimbIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // Store the original gravity scale before modifying it
        originalGravityScale = rb.gravityScale;
        
        // Stop any existing velocity when entering wall climb idle state
        rb.linearVelocity = Vector2.zero;
        
        // Temporarily disable gravity while wall climbing idle
        rb.gravityScale = 0f;
        
        Debug.Log("Entered Wall Climb Idle State - Hanging on wall");
    }

    public override void Exit()
    {
        base.Exit();
        
        // Restore the original gravity scale when exiting wall climb idle state
        rb.gravityScale = originalGravityScale;
        
        Debug.Log("Exited Wall Climb Idle State");
    }

    public override void Update()
    {
        base.Update();
        
        // Exit if jump is pressed (wall jump)
        if (player.GetJumpPressed())
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }
        
        // Exit if player is grounded
        if (player.isGrounded())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
        
        // Exit if no wall is detected
        if (!player.IsWallDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }
        
        // Exit if wall climb is no longer being held
        if (!player.GetWallClimbHeld())
        {
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }
        
        // Exit if player moves away from wall (releases movement towards wall)
        if (xInput != 0 && player.facingDirection != xInput)
        {
            stateMachine.ChangeState(player.airState);
            return;
        }
        
        // Check for climbing movement input to transition to active climbing
        Vector2 climbInput = player.inputHandler.WallClimbMovementInput;
        
        if (Mathf.Abs(climbInput.y) > 0.1f) // Small deadzone for input
        {
            // Player is giving vertical input - transition to active wall climbing
            stateMachine.ChangeState(player.wallClimbState);
            return;
        }
        
        // Stay attached to wall with no movement (hanging/idle on wall)
        // Force velocity to zero every frame to counter any external forces
        rb.linearVelocity = Vector2.zero;
        
        // Ensure player stays facing the wall
        if (xInput > 0 && !player.facingRight)
        {
            player.Flip();
        }
        else if (xInput < 0 && player.facingRight)
        {
            player.Flip();
        }
    }
}
