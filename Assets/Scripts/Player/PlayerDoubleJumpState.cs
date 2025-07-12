using UnityEngine;

public class PlayerDoubleJumpState : PlayerState
{
    public PlayerDoubleJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // Perform the double jump
        player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.jumpForce);
        
        // Mark that double jump has been used
        player.hasUsedDoubleJump = true;
        
        // Play double jump particle effect if available
        if (player.jumpParticle != null)
        {
            player.jumpParticle.Play();
        }
    }

    public override void Update()
    {
        base.Update();
        
        // Allow dash during double jump
        player.CheckForDashInput();
        
        // Apply horizontal movement during double jump
        player.SetVelocity(player.moveSpeed * xInput, player.rb.linearVelocity.y);
        
        // Transition to air state when starting to fall or immediately after jump impulse
        if (player.rb.linearVelocity.y <= 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
