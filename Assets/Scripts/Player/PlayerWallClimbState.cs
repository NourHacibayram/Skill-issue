using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerState
{
    private float climbSpeed = 3f;
    private float transitionCooldown; // Timer to prevent rapid state switching
    //private float staminaDepletion = 0f; // For future stamina system if needed

    public PlayerWallClimbState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        // CHECK IF PLAYER HAS WALL CLIMB SKILL
        if (!PlayerSkills.HasWallClimb())
        {
            // If no wall climb skill, fall normally
            stateMachine.ChangeState(player.airState);
            return;
        }

        // Stop any existing velocity when entering climb state
        rb.linearVelocity = Vector2.zero;
        
        // Set a small cooldown to prevent immediate state switching
        transitionCooldown = 0.05f; // Reduced from 0.1f to be more responsive
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        // Decrease transition cooldown
        if (transitionCooldown > 0)
            transitionCooldown -= Time.deltaTime;

        // Exit if jump is pressed (wall jump)
        if (player.GetJumpPressed())
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        // Exit if player actively moves away from wall (pressing away from wall)
        if (xInput != 0 && player.facingDirection != xInput)
        {
            stateMachine.ChangeState(player.airState);
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

        // Exit if wall climb is no longer being held (stop climbing)
        if (!player.GetWallClimbHeld())
        {
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }
        
        // Climbing movement - use the dedicated ClimbMovement input for vertical movement
        Vector2 climbInput = player.inputHandler.WallClimbMovementInput;
        float verticalInput = climbInput.y;
        
        if (verticalInput > 0.1f) // Climbing up (with deadzone)
        {
            rb.linearVelocity = new Vector2(0, climbSpeed);
        }
        else if (verticalInput < -0.1f) // Climbing down (with deadzone)
        {
            rb.linearVelocity = new Vector2(0, -climbSpeed * 0.5f); // Slower descent
        }
        else if (transitionCooldown <= 0) // Only transition to idle after cooldown and when no significant input
        {
            // Use much smaller threshold for exiting - create a larger "dead zone" to prevent cycling
            // This creates hysteresis: need 0.1f to enter active climb, but only 0.02f to exit to idle
            if (Mathf.Abs(verticalInput) < 0.02f) // Much smaller exit threshold
            {
                stateMachine.ChangeState(player.wallClimbIdleState);
                return;
            }
        }

        // If we're still in transition cooldown or have some input, maintain current velocity
        // This prevents sudden stops when transitioning between states
        
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
