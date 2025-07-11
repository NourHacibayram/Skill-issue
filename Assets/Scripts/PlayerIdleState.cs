using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{   
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.ZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // Use the new input system's MoveInput instead of xInput
        float xInput = player.GetMoveInput().x;

        if(xInput == player.facingDirection && player.IsWallDetected())
            return;

        if (xInput != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
        else
        {
            player.SetVelocity(0, player.rb.linearVelocity.y);
        }
    }
}