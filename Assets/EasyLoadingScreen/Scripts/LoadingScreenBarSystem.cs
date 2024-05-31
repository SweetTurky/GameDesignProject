using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadingScreenBarSystem : MonoBehaviour
{

    public GameObject bar;
    public Text loadingText;
    public bool backGroundImageAndLoop;
    public float LoopTime;
    public GameObject[] backgroundImages;
    [Range(0, 1f)] public float vignetteEfectVolue; // Must be a value between 0 and 1
    public AsyncOperation async;
    Image vignetteEfect;


    public void loadingScreen(int sceneNo)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(Loading(sceneNo));
    }

    // Used to try. Delete the comment lines (25 and 36)
    /*
    public void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            bar.transform.localScale += new Vector3(0.001f,0,0);

            if (loadingText != null)
                loadingText.text = "%" + (100 * bar.transform.localScale.x).ToString("####");
        }
    }
    */

    private void Start()
    {
        vignetteEfect = transform.Find("VignetteEfect").GetComponent<Image>();
        vignetteEfect.color = new Color(vignetteEfect.color.r, vignetteEfect.color.g, vignetteEfect.color.b, vignetteEfectVolue);

        if (backGroundImageAndLoop)
            StartCoroutine(transitionImage());
    }


    // The pictures change according to the time of
    IEnumerator transitionImage()
    {
        for (int i = 0; i < backgroundImages.Length; i++)
        {
            yield return new WaitForSeconds(LoopTime);
            for (int j = 0; j < backgroundImages.Length; j++)
                backgroundImages[j].SetActive(false);
            backgroundImages[i].SetActive(true);
        }
    }

    // Activate the scene 
    IEnumerator Loading(int sceneNo)
    {
        yield return new WaitForSeconds(1);
        async = SceneManager.LoadSceneAsync(sceneNo);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            // Calculate the fill amount based on loading progress (async.progress)
            float fillAmount = Mathf.Clamp01(async.progress / 0.9f); // 0.9f is when the scene is almost loaded

            // Update the fill amount of the bar
            bar.GetComponent<Image>().fillAmount = fillAmount;

            if (loadingText != null)
                loadingText.text = "Loading %" + (fillAmount * 100).ToString("####");

            // If loading is almost complete, adjust the fill amount to 1 and allow scene activation
            if (async.progress >= 0.9f)
            {
                bar.GetComponent<Image>().fillAmount = 1;
                async.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}
