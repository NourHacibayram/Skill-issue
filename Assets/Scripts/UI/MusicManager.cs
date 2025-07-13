using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip[] tracks;
    }
    
    [Header("Scene Music")]
    [SerializeField] private SceneMusic[] sceneMusicList;
    [SerializeField] private float volume = 0.4f;
    
    private AudioSource audioSource;
    private static MusicManager instance;
    
    // Public access to the instance
    public static MusicManager Instance 
    { 
        get 
        { 
            return instance; 
        } 
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Ensure we have an AudioSource component
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // Configure AudioSource settings
            audioSource.playOnAwake = false;
            audioSource.loop = true;
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            PlayMusicForCurrentScene();
        }
        else if (instance != this)
        {
            // If there's already an instance and it's not this one, destroy this one
            Destroy(gameObject);
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"MusicManager: Scene loaded - {scene.name}");
        PlayMusicForScene(scene.name);
    }
    
    private void PlayMusicForScene(string sceneName)
    {
        // Check if audioSource is still valid
        if (audioSource == null || this == null)
        {
            Debug.LogError("MusicManager: AudioSource or MusicManager is null!");
            return;
        }
        
        Debug.Log($"MusicManager: Looking for music for scene '{sceneName}'");
        
        foreach (var sceneMusic in sceneMusicList)
        {
            if (sceneMusic.sceneName == sceneName && sceneMusic.tracks.Length > 0)
            {
                AudioClip randomTrack = sceneMusic.tracks[Random.Range(0, sceneMusic.tracks.Length)];
                Debug.Log($"MusicManager: Playing track '{randomTrack.name}' for scene '{sceneName}'");
                audioSource.clip = randomTrack;
                audioSource.volume = volume;
                audioSource.loop = true;
                audioSource.Play();
                return;
            }
        }
        
        Debug.Log($"MusicManager: No music found for scene '{sceneName}', stopping music");
        // Stop music if no music found for this scene
        audioSource.Stop();
    }
    
    private void PlayMusicForCurrentScene()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }
    
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (audioSource != null && this != null)
            audioSource.volume = volume;
    }
    
    public void StopMusic()
    {
        if (audioSource != null && this != null)
            audioSource.Stop();
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from scene events to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        // Clear the instance reference if this is the current instance
        if (instance == this)
        {
            instance = null;
        }
        
        Debug.Log("MusicManager: Instance destroyed");
    }
    
    // Method to validate the MusicManager state
    public void ValidateState()
    {
        Debug.Log($"MusicManager State Check:");
        Debug.Log($"- Instance exists: {instance != null}");
        Debug.Log($"- AudioSource exists: {audioSource != null}");
        Debug.Log($"- GameObject active: {gameObject != null && gameObject.activeInHierarchy}");
        Debug.Log($"- Scene music list count: {sceneMusicList?.Length ?? 0}");
        
        if (audioSource != null)
        {
            Debug.Log($"- AudioSource playing: {audioSource.isPlaying}");
            Debug.Log($"- AudioSource clip: {(audioSource.clip != null ? audioSource.clip.name : "null")}");
        }
    }
}