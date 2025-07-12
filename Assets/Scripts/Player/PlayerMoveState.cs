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

        player.SetVelocity(player.moveSpeed * xInput, player.rb.linearVelocity.y);
        Debug.Log("Velocity: " + player.rb.linearVelocity);
        Debug.Log("xInput: " + xInput);

        if (xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}