using UnityEngine;

public class VoidZone : MonoBehaviour
{
    [Header("Void Settings")]
    [SerializeField] private AudioClip voidFallSound;
    [SerializeField] private float voidThreshold = -10f; // Y position that triggers void death
    
    private AudioSource audioSource;
    
    private void Start()
    {
        // Get or add audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerVoidDeath(other.GetComponent<Player>());
        }
    }
    
    // Alternative method for checking Y position threshold
    private void Update()
    {
        // Find player and check if they've fallen below the void threshold
        Player player = FindFirstObjectByType<Player>();
        if (player != null && player.transform.position.y < voidThreshold)
        {
            TriggerVoidDeath(player);
        }
    }
    
    private void TriggerVoidDeath(Player player)
    {
        if (player != null && player.stateMachine.currentState != player.deadState)
        {
            // Set death type to void
            player.deadState.SetDeathType(PlayerDeadState.DeathType.Void);
            
            // Play void fall sound
            PlayVoidSound();
            
            // Transition to dead state
            player.stateMachine.ChangeState(player.deadState);
        }
    }
    
    private void PlayVoidSound()
    {
        if (voidFallSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(voidFallSound);
        }
    }
}
