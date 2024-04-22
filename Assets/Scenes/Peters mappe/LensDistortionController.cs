using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class LensDistortionController : MonoBehaviour
{
    public Volume volume; // Reference to the Volume component attached to the camera
    public float minIntensity = 0f; // Minimum intensity of lens distortion
    public float maxIntensity = 1f; // Maximum intensity of lens distortion
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
        if (sanityManager != null)
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
        // Use a sine wave function to interpolate between minIntensity and maxIntensity continuously
        float frequency = 1f; // Adjust the frequency of the oscillation
        float phase = Mathf.Sin(2f * Mathf.PI * frequency * Time.time + timeOffset);
        return Mathf.InverseLerp(-1f, 1f, phase);
    }
}
