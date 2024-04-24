using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    public float fadeDuration = 2.0f; // Duration of the fade effect in seconds

    public Image canvasImage;
    private Color originalColor;
    private Color targetColor = Color.black;

    void Start()
    {
        originalColor = canvasImage.color;
    }

    public void StartFade()
    {
        StartCoroutine(FadeCanvas());
    }

    IEnumerator FadeCanvas()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / fadeDuration;

            // Interpolate the alpha channel from 0 to 1
            float alpha = Mathf.Lerp(0f, 1f, normalizedTime);

            // Set the alpha channel to the maximum value (255)
            Color newColor = new Color(targetColor.r, targetColor.g, targetColor.b, alpha);

            if (canvasImage != null)
                canvasImage.color = newColor;
            else
                yield break; // Exit the coroutine if the Image component is null
        }

        // Ensure the final color is fully opaque
        canvasImage.color = targetColor;
    }
}