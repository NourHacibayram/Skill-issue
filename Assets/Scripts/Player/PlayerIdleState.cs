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

        float xInput = player.GetMoveInput().x;

        if (xInput == player.facingDirection && player.IsWallDetected())
            return;

        // Check for dash input
        if (player.GetDashPressed() && !player.isBusy)
        {
            // Set dash direction
            if (xInput != 0)
                player.SetDashDirection(Mathf.Sign(xInput));
            else
                player.SetDashDirection(player.facingDirection);

            stateMachine.ChangeState(player.dashState);
            return;
        }

        if (xInput != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
        else
        {
            player.SetVelocity(0, player.rb.linearVelocity.y);
        }
        
        if(player.IsWallDetected() && player.GetWallClimbPressed())
        {
            stateMachine.ChangeState(player.wallClimbState);
            return;
        }
    }
}
