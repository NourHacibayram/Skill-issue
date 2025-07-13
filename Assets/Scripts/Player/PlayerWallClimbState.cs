using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerState
{
    private float climbSpeed = 3f;
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
        Debug.Log("Entered Wall Climb State");
    }
    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exited Wall Climb State");
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

        // Exit if player moves away from wall (releases movement towards wall)
        if (xInput != 0 && player.facingDirection != xInput)
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        // Exit if player is not pressing towards the wall anymore
        if (xInput == 0)
        {
            stateMachine.ChangeState(player.wallSlideState);
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
        
        // Exit if player moves away from wall (releases movement towards wall)
        if (xInput != 0 && player.facingDirection != xInput)
        {
            stateMachine.ChangeState(player.airState);
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
        else // No significant vertical input - transition to idle climbing state
        {
            stateMachine.ChangeState(player.wallClimbIdleState);
            return;
        }

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
