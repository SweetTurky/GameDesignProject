using UnityEngine;
using UnityEngine.UI;

public class SanityBar : MonoBehaviour
{
    public Slider sanitySlider;
    public Slider lightSlider;
    public SanityManager sanityManager; // Reference to your SanityManager script
    public ObjectGrabbable objectGrabbable;

    void Start()
    {
        sanitySlider.maxValue = 100f; // Set the maximum value of the Slider
        sanitySlider.value = sanityManager.playerSanity; // Set the initial value of the Slider to the player's current sanity

        lightSlider.maxValue = 60f;
        lightSlider.value = sanityManager.candleTimeLeft;
        //UpdateLightBar();
    }

    void Update()
    {
        // Update the value of the Slider to match the player's current sanity
        sanitySlider.value = sanityManager.playerSanity;

        // Update the value of the lightSlider to match the timer value
        lightSlider.value = sanityManager.candleTimeLeft;

        // Update the light bar visual representation based on the remaining light time
        //UpdateLightBar();
    }

    public void UpdateLightBar()
        {
            // Calculate the normalized value for the light slider
            float normalizedLightTime = sanityManager.candleTimeLeft / 60f;

            // Update the fill amount of the light slider
            lightSlider.value = normalizedLightTime * 60f;
        }

}
