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
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            PlayMusicForCurrentScene();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }
    
    private void PlayMusicForScene(string sceneName)
    {
        foreach (var sceneMusic in sceneMusicList)
        {
            if (sceneMusic.sceneName == sceneName && sceneMusic.tracks.Length > 0)
            {
                AudioClip randomTrack = sceneMusic.tracks[Random.Range(0, sceneMusic.tracks.Length)];
                audioSource.clip = randomTrack;
                audioSource.volume = volume;
                audioSource.loop = true;
                audioSource.Play();
                return;
            }
        }
        
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
        if (audioSource != null)
            audioSource.volume = volume;
    }
    
    public void StopMusic()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
}