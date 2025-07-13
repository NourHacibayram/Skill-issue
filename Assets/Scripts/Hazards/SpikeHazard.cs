using UnityEngine;

public class SpikeHazard : MonoBehaviour
{
    [Header("Spike Settings")]
    [SerializeField] private AudioClip spikeHitSound;
    [SerializeField] private GameObject spikeEffect; // Optional visual effect
    
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
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // Set death type to spike
                player.deadState.SetDeathType(PlayerDeadState.DeathType.Spike);
                
                // Play spike hit sound
                PlaySpikeSound();
                
                // Show spike effect if available
                if (spikeEffect != null)
                {
                    Instantiate(spikeEffect, other.transform.position, Quaternion.identity);
                }
                
                // Transition to dead state
                player.stateMachine.ChangeState(player.deadState);
            }
        }
    }
    
    private void PlaySpikeSound()
    {
        if (spikeHitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spikeHitSound);
        }
    }
}
