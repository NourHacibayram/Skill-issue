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

        if (player.jumpState.CanDoubleJump())
        {
            Debug.Log("Double Jumping!");
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.doubleJumpForce);
            player.anim.SetBool("DoubleJump", true);
            player.jumpState.UseJump(); // Reduce jump count
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.anim.SetBool("DoubleJump", false); // Ensure animation stops
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
            player.anim.SetBool("DoubleJump", false);
            player.anim.Play("PlayerIdle", 0); 
        }
    }
}
