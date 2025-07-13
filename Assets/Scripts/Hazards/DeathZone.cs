using UnityEngine;

/// <summary>
/// Simple death zone trigger that kills the player when they enter it.
/// Place this at the bottom of your level to catch players who fall off the map.
/// </summary>
public class DeathZone : MonoBehaviour
{
    [Header("Death Zone Settings")]
    [Tooltip("Type of death this zone causes")]
    public PlayerDeadState.DeathType deathType = PlayerDeadState.DeathType.Void;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player.stateMachine.currentState != player.deadState)
            {
                // Set the death type
                player.deadState.SetDeathType(deathType);
                
                // Transition to dead state
                player.stateMachine.ChangeState(player.deadState);
                
                Debug.Log($"Player died in death zone: {deathType}");
            }
        }
    }
}
