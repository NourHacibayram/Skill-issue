using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        // Check for dash input - ONLY if dash is selected skill
        if (player.GetDashPressed() && !player.isBusy && PlayerSkills.HasDash())
        {
            // Set dash direction based on movement
            if (xInput != 0)
                player.SetDashDirection(Mathf.Sign(xInput));
            else
                player.SetDashDirection(player.facingDirection);

            stateMachine.ChangeState(player.dashState);
            return;
        }

        player.SetVelocity(player.moveSpeed * xInput, player.rb.linearVelocity.y);

        if (xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}