using UnityEngine;
using UnityEngine.UI;

public class SanityBar : MonoBehaviour
{
    public Slider slider;
    public SanityManager sanityManager; // Reference to your SanityManager script

    void Start()
    {
        slider.maxValue = 100f; // Set the maximum value of the Slider
        slider.value = sanityManager.playerSanity; // Set the initial value of the Slider to the player's current sanity
    }

    void Update()
    {
        // Update the value of the Slider to match the player's current sanity
        slider.value = sanityManager.playerSanity;
    }
}

