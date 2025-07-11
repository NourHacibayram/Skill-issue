using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        player.CheckForDashInput();

        // Declare moveInput once at the top of the method
        Vector2 moveInput = player.GetMoveInput();

        // Basic wall slide trigger - only when in air and touching wall
        if (player.IsWallDetected() && !player.isGrounded())
        {
            // Use the moveInput variable declared above - don't redeclare it
            if (moveInput.x == 0 || Mathf.Sign(moveInput.x) == player.facingDirection)
            {
                stateMachine.ChangeState(player.wallSlide);
                return;
            }
        }        if (player.isGrounded())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        // Use the same moveInput variable - don't redeclare it
        if (moveInput.x != 0)
        {
            player.SetVelocity(player.moveSpeed * 0.8f * moveInput.x, rb.linearVelocity.y);
        }
        else
        {
            // Apply air deceleration to gradually slow down but preserve some momentum
            float currentXVelocity = rb.linearVelocity.x;
            float airDeceleration = player.Stats.AirDeceleration;
            float newXVelocity = Mathf.MoveTowards(currentXVelocity, 0, airDeceleration * Time.deltaTime);
            player.SetVelocity(newXVelocity, rb.linearVelocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}