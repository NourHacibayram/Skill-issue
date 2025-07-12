using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
       
    }

    public override void Update()
    {
        base.Update();

        player.CheckForDashInput();

        if (!player.isGrounded())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (player.GetJumpPressed() && player.isGrounded())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}