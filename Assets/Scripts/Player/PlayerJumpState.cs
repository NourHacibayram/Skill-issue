using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private int amountOfJumpsLeft;

    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Ensure jumps are set correctly when first jumping
        if (player.isGrounded())
        {
            ResetJumps();
        }

        if (amountOfJumpsLeft > 0)
        {
            Debug.Log("Jumping! Jumps left: " + amountOfJumpsLeft);
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.jumpForce);
            UseJump(); // Decrease jump count
        }
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.moveSpeed * player.GetMoveInput().x, player.rb.linearVelocity.y);

        if (player.rb.linearVelocity.y < 0 && !player.isGrounded())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (player.GetJumpPressed() && CanDoubleJump())
        {
            stateMachine.ChangeState(player.doubleJumpState);
        }
    }

    public void ResetJumps()
    {
        amountOfJumpsLeft = (int)player.maxJumps; // Resets when landing
    }

    public bool CanDoubleJump()
    {
        return amountOfJumpsLeft > 0;
    }

    public void UseJump()
    {
        if (amountOfJumpsLeft > 0)
        {
            amountOfJumpsLeft--;
        }
    }
}
