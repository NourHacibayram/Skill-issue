/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }    public override void Update()
    {
        base.Update();

        // Get input once at the beginning
        Vector2 moveInput = player.GetMoveInput();

        // Exit immediately if grounded - can't wall slide on ground
        if (player.isGrounded())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }        // Exit wall slide if no longer touching wall
        if (!player.IsWallDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        // Handle all jump scenarios first
        if (player.GetJumpPressed())
        {
            // Always go to wall jump state when jump is pressed - let it handle direction
            stateMachine.ChangeState(player.wallJump);
            return;
        }

        // Exit wall slide if player moves away from wall (only when NOT jumping)
        if (moveInput.x != 0 && player.facingDirection != Mathf.Sign(moveInput.x))
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        // Apply wall slide movement - slide down slowly, faster if pressing down
        float slideSpeed = player.Stats.WallSlideSpeed;
        
        // If player is pressing down, slide faster
        if (moveInput.y < 0)
        {
            slideSpeed *= 2.5f; // Slide 2.5x faster when pressing down
        }
        
        player.SetVelocity(0, -slideSpeed);
    }
} */