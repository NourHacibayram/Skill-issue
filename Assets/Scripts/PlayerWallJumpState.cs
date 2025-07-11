using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    private float controlLockTime = 0.05f; // Time to lock player control
    private float controlLockTimer;

    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }    public override void Enter()
    {
        base.Enter();

        stateTimer = 0.15f; // Even shorter for more responsive control
        controlLockTimer = controlLockTime; // Initialize control lock

        // Get player input to determine wall jump direction
        Vector2 moveInput = player.GetMoveInput();
        
        Debug.Log($"Wall Jump Input - X: {moveInput.x}, FacingDirection: {player.facingDirection}");
        
        // Determine jump direction
        float jumpDirection;
        if (moveInput.x != 0 && player.facingDirection != Mathf.Sign(moveInput.x))
        {
            // Player is pressing away from wall - jump in input direction
            jumpDirection = Mathf.Sign(moveInput.x);
            Debug.Log($"Directional wall jump - Input direction: {jumpDirection}");
        }
        else
        {
            // Regular wall jump - jump away from wall (opposite of facing direction)
            jumpDirection = -player.facingDirection;
            Debug.Log($"Regular wall jump - Away from wall: {jumpDirection}");
        }

        // Use the custom physics system instead of SetVelocity
        float horizontalForce = player.Stats.WallJumpForceX * jumpDirection;
        float verticalForce = player.Stats.WallJumpForceY;

        // Set velocity directly through the public _frameVelocity
        player._frameVelocity = new Vector2(horizontalForce, verticalForce);

        Debug.Log($"Wall jump executed - Direction: {jumpDirection}, Horizontal: {horizontalForce}, Vertical: {verticalForce}");
    }    public override void Update()
    {
        base.Update();

        // Countdown the control lock timer
        controlLockTimer -= Time.deltaTime;

        // Only allow player input after control lock expires
        if (controlLockTimer <= 0)
        {
            // Allow player input during wall jump for better control
            Vector2 moveInput = player.GetMoveInput();
            if (moveInput.x != 0)
            {
                // Allow some air control during wall jump
                float airControl = player.Stats.Acceleration * 0.5f;
                float targetSpeed = moveInput.x * player.Stats.MaxSpeed * 0.8f;
                player._frameVelocity.x = Mathf.MoveTowards(player._frameVelocity.x, targetSpeed, airControl * Time.deltaTime);
            }
        }

        if (stateTimer <= 0)
            stateMachine.ChangeState(player.airState);

        if (player.isGrounded())
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        // Don't reset velocity here - let the custom physics handle it
    }
}