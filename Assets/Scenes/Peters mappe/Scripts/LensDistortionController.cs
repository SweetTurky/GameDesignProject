using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class LensDistortionController : MonoBehaviour
{
    public Volume volume; // Reference to the Volume component attached to the camera
    public float minIntensity = -0.2f; // Minimum intensity of lens distortion
    public float maxIntensity = 0.7f; // Maximum intensity of lens distortion
    private LensDistortion lensDistortion; // Reference to the LensDistortion component

    // Reference to the player's sanity manager (assuming you have one)
    public SanityManager sanityManager;

    private float timeOffset = 0f; // Offset to control the phase of the sine wave

    private void Start()
    {
        // Get the LensDistortion component from the Volume component attached to the camera
        VolumeProfile profile = volume.sharedProfile;
        profile.TryGet(out lensDistortion);

        // Ensure we have a reference to the LensDistortion component
        if (lensDistortion == null)
        {
            Debug.LogError("Lens Distortion component not found!");
            enabled = false; // Disable the script if LensDistortion component is not found
        }
    }

    private void Update()
    {
        if (sanityManager != null && sanityManager.playerSanity < 80)
        {
            // Calculate the intensity offset based on the player's sanity level
            float sanityPercentage = sanityManager.GetSanityPercentage();
            float intensityOffset = GetIntensityOffset(sanityPercentage);

            // Update the intensity of the lens distortion effect
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, intensityOffset);
            lensDistortion.intensity.value = intensity;
        }
    }

    private float GetIntensityOffset(float sanityPercentage)
    {
        float inputRange = 2f; // Adjust this value to control the range for each sanity level
        float inputMin = ((100f - inputRange) / 100f);
        float inputMax = (1f - inputMin);
        float normalizedSanity = Mathf.InverseLerp(inputMin, inputMax, (sanityPercentage * 1f));

         if(sanityPercentage < 50)
        {
            normalizedSanity = normalizedSanity * 0.4f;
        } 
        else if(sanityPercentage < 40)
        {
            normalizedSanity = normalizedSanity * 0.35f;
        } 
        else if(sanityPercentage < 30)
        {
            normalizedSanity = normalizedSanity * 0.3f;
        } 
        else if(sanityPercentage < 20)
        {
            normalizedSanity = normalizedSanity * 0.25f;
        } 
        else if(sanityPercentage < 10)
        {
            normalizedSanity = normalizedSanity * 0.2f;
        } 

        float targetMin = 0f;
        float targetMax = 0f;

        // Define the target intensity range based on sanity percentage
        if (sanityPercentage >= 75f)
        {
            targetMin = 0f;
            targetMax = 0f;
        }
        else if (sanityPercentage >= 50f)
        {
            targetMin = -0.1f;
            targetMax = 0.1f;
        }
        else if (sanityPercentage >= 25f)
        {
            targetMin = -0.15f;
            targetMax = 0.15f;
        }
        else
        {
            targetMin = -0.2f;
            targetMax = 0.2f;
        }

        // Interpolate between the target intensity range based on the sine wave
        float frequency = 0.25f; // Adjust the frequency of the oscillation
        float phase = Mathf.Sin(4f * Mathf.PI * frequency * Time.time + timeOffset);
        float intensityOffset = Mathf.Lerp(targetMin, targetMax, (phase + 1f) / 2f);

        return Mathf.Lerp(normalizedSanity, intensityOffset, Mathf.Abs(phase)); // Use the sine wave to interpolate between normalized sanity and intensity offset
    }
}
