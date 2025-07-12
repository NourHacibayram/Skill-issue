// filepath: c:\Users\yazan\Desktop\Work\Caelum\Assets\Scripts\Player\PlayerJumpState.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private int amountOfJumpsLeft;

    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // Let the custom physics handle the jump velocity instead
        player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
        player.anim.SetBool("Jump", false);
    }

    public override void Update()
    {
        base.Update();
        player.CheckForDashInput();
        
        player.SetVelocity(player.moveSpeed * xInput, player.rb.linearVelocity.y);
            
        if(player.rb.linearVelocity.y <= 0 && !player.isGrounded())
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}