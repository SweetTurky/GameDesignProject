using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance; // Singleton instance

    public Image fadeImage; // Reference to the UI Image used for fading
    public float fadeSpeed = 5f; // Speed of the fade

    private bool isFading = false; // Flag to check if fade is currently happening

    // Event to be triggered when the fade completes
    public event Action OnFadeComplete;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Ensure that the fade image is transparent initially
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    // Function to fade to black
    public void FadeToBlack()
    {
        StartCoroutine(Fade(true));
    }

    // Function to fade from black
    public void FadeFromBlack()
    {
        StartCoroutine(Fade(false));
    }

    IEnumerator Fade(bool fadeIn)
    {
        if (isFading)
            yield break;

        isFading = true;

        // Fade in
        if (fadeIn)
        {
            while (fadeImage.color.a < 0.99f)
            {
                float alpha = fadeImage.color.a + (fadeSpeed * Time.deltaTime);
                fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return null;
            }
        }
        // Fade out
        else
        {
            while (fadeImage.color.a > 0f)
            {
                float alpha = fadeImage.color.a - (fadeSpeed * Time.deltaTime);
                fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
                yield return null;
            }
        }

        isFading = false;

        // Trigger the OnFadeComplete event
        OnFadeComplete?.Invoke();
    }
}
