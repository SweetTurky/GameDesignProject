using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneSwitcher : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName;
    public LoadingScreenBarSystem loadingScreenBarSystem;

    private bool sceneSwitched = false;

    void Start()
    {
        // Subscribe to the loopPointReached event
        videoPlayer.loopPointReached += OnLoopPointReached;
    }

    private void Update()
    {
        // Check if the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter pressed");
            videoPlayer.enabled = false;
            
            // Skip to the next scene
            loadingScreenBarSystem.loadingScreen(2);
        }
    }

    void OnLoopPointReached(VideoPlayer vp)
    {
        // Check if the scene hasn't been switched yet
        if (!sceneSwitched)
        {
            // Switch scene
            loadingScreenBarSystem.loadingScreen(2);
            sceneSwitched = true;
        }
    }
}
