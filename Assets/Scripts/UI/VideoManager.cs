using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoManager : MonoBehaviour
{
    [Header("Scene Management")]
    [SerializeField] private string firstLevelSceneName = "Scenes/Levels/Level_01"; [SerializeField] private bool allowSkip = true;
    [SerializeField] private KeyCode skipKey = KeyCode.Space;

    [Header("UI (Optional)")]
    [SerializeField] private GameObject skipText; // Optional "Press SPACE to skip" text

    private VideoPlayer videoPlayer;
    private bool videoFinished = false;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component not found!");
            LoadFirstLevel(); // Fallback
            return;
        }

        // Subscribe to video end event
        videoPlayer.loopPointReached += OnVideoFinished;

        // Show skip text if available
        if (skipText != null)
            skipText.SetActive(allowSkip);

        // Start playing video
        videoPlayer.Play();

        Debug.Log("Cutscene video started");
    }

    void Update()
    {
        // Allow skipping video
        if (allowSkip && Input.GetKeyDown(skipKey) && !videoFinished)
        {
            SkipVideo();
        }

        // Fallback: If video stops unexpectedly
        if (!videoPlayer.isPlaying && !videoFinished && videoPlayer.time > 0)
        {
            OnVideoFinished(videoPlayer);
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (videoFinished) return; // Prevent multiple calls

        videoFinished = true;
        Debug.Log("Video finished - loading first level");

        StartCoroutine(LoadFirstLevelWithDelay());
    }

    void SkipVideo()
    {
        Debug.Log("Video skipped by player");
        videoPlayer.Stop();
        OnVideoFinished(videoPlayer);
    }

    IEnumerator LoadFirstLevelWithDelay()
    {
        // Optional: Add fade out or delay
        yield return new WaitForSeconds(1f);

        LoadFirstLevel();
    }

    void LoadFirstLevel()
    {
        Debug.Log($"Loading first level: {firstLevelSceneName}");
        SceneManager.LoadScene(firstLevelSceneName);
    }
}