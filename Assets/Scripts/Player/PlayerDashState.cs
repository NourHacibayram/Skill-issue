using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        // Continue dashing
        player.SetVelocity(player.dashSpeed * player.dashDirection, 0);

        // Check for state transitions after dash
        if(stateTimer <= 0)
        {
            // Determine next state based on conditions
            if (player.isGrounded())
            {
                if (player.GetMoveInput().x != 0)
                    stateMachine.ChangeState(player.moveState);
                else
                    stateMachine.ChangeState(player.idleState);
            }
            else
            {
                if (player.IsWallDetected())
                    stateMachine.ChangeState(player.wallSlideState);
                else
                    stateMachine.ChangeState(player.airState);
            }
        }
    }
}