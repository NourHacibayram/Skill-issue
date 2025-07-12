// using System.Collections;
// using System.Collections.Generic;
// // using System.Numerics;
// using Unity.VisualScripting;
// using UnityEngine;

// public class PlayerDashState : PlayerState
// {
//     public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
//     {
//     }

//     public override void Enter()
//     {
//         base.Enter();
//         stateTimer = player.dashDuration;
//     }

//     public override void Exit()
//     {
//         base.Exit();
//         player.SetVelocity(0, rb.linearVelocity.y);
//     }

//     public override void Update()
//     {
//         base.Update();

//         // Spawn AfterImage at proper intervals based on distance (both X and Y)
//         float distanceX = Mathf.Abs(player.lastImageXpos - player.transform.position.x);
//         float distanceY = Mathf.Abs(player.lastImageYpos - player.transform.position.y);
//         float totalDistance = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);

//         if (totalDistance > player.disntaceBetweenImages)
//         {
//             // PlayerAfterImagePool.instance.GetFromPool();
//             player.lastImageXpos = player.transform.position.x;
//             player.lastImageYpos = player.transform.position.y;
//         }

//         // Use both horizontal and vertical dash directions
//         UnityEngine.Vector2 dashVelocity = new UnityEngine.Vector2(
//             player.dashSpeed * player.dashDirection,
//             player.dashSpeed * player.dashDirectionY
//         );

//         // Normalize diagonal dashes so they don't go faster than straight dashes
//         if (dashVelocity.magnitude > player.dashSpeed)
//         {
//             dashVelocity = dashVelocity.normalized * player.dashSpeed;
//         }

//         player.SetVelocity(dashVelocity.x, dashVelocity.y);

//         if (stateTimer <= 0)
//             stateMachine.ChangeState(player.idleState);
//     }
// }