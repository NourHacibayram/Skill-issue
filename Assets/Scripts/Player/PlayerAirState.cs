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

        // Check for double jump input when in air
        if (player.GetJumpPressed() && player.jumpState.CanDoubleJump())
        {
            stateMachine.ChangeState(player.doubleJumpState);
            return;
        }

        // Check for dash input (air dash) - ONLY if dash is selected skill
        if (player.GetDashPressed() && !player.isBusy && PlayerSkills.HasDash())
        {
            // Set dash direction
            if (xInput != 0)
                player.SetDashDirection(Mathf.Sign(xInput));
            else
                player.SetDashDirection(player.facingDirection);

            stateMachine.ChangeState(player.dashState);
            return;
        }

        // Check for wall climb input when facing a wall
        if (player.GetWallClimbPressed() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallClimbIdleState);
            return;
        }
        
        // Check for ledge grab when falling and near a ledg

        if (player.IsWallDetected() && !player.GetWallClimbPressed())
            stateMachine.ChangeState(player.wallSlideState);

        if (player.isGrounded())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (xInput != 0)
        {
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.linearVelocity.y);
        }
        else
        {
            player.SetVelocity(0, rb.linearVelocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}