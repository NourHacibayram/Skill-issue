using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : PlayerState
{
    public PlayerDoubleJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
        : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        // CHECK IF PLAYER HAS DOUBLE JUMP SKILL
        if (!PlayerSkills.HasDoubleJump())
        {
            // If no double jump skill, go back to falling
            stateMachine.ChangeState(player.airState);
            return;
        }

        if (player.jumpState.CanDoubleJump())
        {
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.doubleJumpForce);
            player.PlayJumpSound();
            player.jumpState.UseJump();
        }
    }
    public override void Exit()
    {
        base.Exit();
        player.anim.SetBool("Jump", false); // Ensure animation stops
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.moveSpeed * player.GetMoveInput().x, player.rb.linearVelocity.y);

        if (player.rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }

    public void EndDoubleJumpAnimation()
    {
        if (stateMachine.currentState == this)
        {
            player.anim.SetBool("Jump", false);
            player.anim.Play("PlayerIdle", 0);
        }
    }
}
