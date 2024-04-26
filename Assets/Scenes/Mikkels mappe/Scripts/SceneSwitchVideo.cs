using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneSwitcher : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName;

    private bool sceneSwitched = false;

    void Start()
    {
        // Subscribe to the loopPointReached event
        videoPlayer.loopPointReached += OnLoopPointReached;
    }

    void OnLoopPointReached(VideoPlayer vp)
    {
        // Check if the scene hasn't been switched yet
        if (!sceneSwitched)
        {
            // Switch scene
            SceneManager.LoadScene(nextSceneName);
            sceneSwitched = true;
        }
    }
}
