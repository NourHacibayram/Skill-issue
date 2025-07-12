using System.Collections;
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
    }

    public override void Update()
    {
        base.Update();
        
        if (player.GetJumpPressed())
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        if (xInput != 0 && player.facingDirection != xInput)
        {
            stateMachine.ChangeState(player.moveState);
            return;
        }

        if (player.isGrounded())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        if (!player.IsWallDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        float slideSpeed = -2f;
        rb.linearVelocity = new Vector2(0, slideSpeed);
    }
}
