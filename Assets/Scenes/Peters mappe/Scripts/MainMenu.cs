using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        FadeManager.instance.FadeToBlack();
        // Directly trigger the scene switch when the fade completes
        FadeManager.instance.OnFadeComplete += () => SceneManager.LoadScene("Scene_combine1");
    }

    public void ExitGame()
    {
        FadeManager.instance.FadeToBlack();
        // Directly quit the application when the fade completes
        FadeManager.instance.OnFadeComplete += Application.Quit;
    }

    public void BackToMainMenu()
    {
        FadeManager.instance.FadeToBlack();
        // Directly trigger the scene switch when the fade completes
        FadeManager.instance.OnFadeComplete += () => SceneManager.LoadScene("MainMenu");
    }
}
