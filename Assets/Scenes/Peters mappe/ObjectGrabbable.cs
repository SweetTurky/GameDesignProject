using UnityEngine;
using System.Collections;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private bool firstPickup = true;
    private float lightTimer = 0f;
    private float initialIntensity = 0f;
    public Light lanternLight;
    public SanityManager sanityManager;
    public float lightTimerIncreaseAmount = 20f;
    public SanityBar sanityBar;
    bool lightRestored = false; // Flag to track if light has been restored
    float elapsedTime = 0f;
    private bool candleTimeDecreasedThisFrame = false;
    private bool firstCandlePickup = true;


    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        lanternLight = GetComponentInChildren<Light>();
        initialIntensity = lanternLight.intensity;
        lightTimer = sanityManager.candleTimeLeft; 
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        GameManager.instance.lanternInstruction.SetActive(false); // Disable instruction when the lantern is grabbed

        if (firstPickup)
        {
            Debug.Log("First pickup");
            firstPickup = false;
            lightTimer = 60f; // Set the timer to 60 seconds
            StartCoroutine(FadeLightIntensity()); // Start fading the light intensity
        }
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;

        // Show instruction when the lantern is dropped and player is looking at it
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, GameManager.instance.pickUpDistance, GameManager.instance.pickUpLayerMask))
        {
            if (hit.transform.CompareTag("Lantern"))
            {
                GameManager.instance.lanternInstruction.SetActive(true);
            }
        }
    }
    public void IncreaseLightTimer()
    {
        //lightTimer = sanityManager.candleTimeLeft;
        Debug.Log(sanityManager.candleTimeLeft);
        if (sanityManager.candleTimeLeft < 60f)
        {
            sanityManager.candleTimeLeft += 5f;

            lightTimer = sanityManager.candleTimeLeft; // Update lightTimer to reflect the change
            Debug.Log(sanityManager.candleTimeLeft);
            StartCoroutine(FadeLightIntensity());
        }
        lanternLight.intensity = initialIntensity;
    }


   public IEnumerator FadeLightIntensity()
    {
        float fadeDuration = sanityManager.candleTimeLeft;
        elapsedTime = 0f;
        
        while (elapsedTime < fadeDuration)
        {
            // Calculate the progress based on the elapsed time
            float progress = elapsedTime / fadeDuration;

            // Calculate the new intensity based on the progress
            float newIntensity = Mathf.Lerp(initialIntensity, 0f, progress);

            // Set the light intensity
            lanternLight.intensity = newIntensity;

            // Check if the light has reached 0 intensity and has not been restored yet
            if (newIntensity <= 0f && !lightRestored)
            {
                // Restore the light intensity
                RestoreLight();
                lightRestored = true; // Set the flag to true to prevent further restoration
            }

            if (!candleTimeDecreasedThisFrame)
            {
                DecreaseCandleTime();
                candleTimeDecreasedThisFrame = true; // Set the flag to true to indicate that the decrease has been applied
            }
            else
            {
                candleTimeDecreasedThisFrame = false; // Reset the flag for the next frame
            }

            // Wait for the next frame
            yield return null;

            // Update the elapsed time
            elapsedTime += Time.deltaTime;
        }

        // Ensure light intensity is set to 0
        lanternLight.intensity = 0f;

        // Deactivate the light when the timer reaches 0
        if (lightTimer <= 0f)
        {
            lanternLight.enabled = false;
        }
    }
    private void RestoreLight()
    {
    // Restore the light intensity
    lanternLight.intensity = initialIntensity;
    lanternLight.enabled = true;
    // Reset the flag when the light is restored
    candleTimeDecreasedThisFrame = false;
    }
    private void DecreaseCandleTime()
    {
        //sanityManager.candleTimeLeft -= Time.deltaTime;
    }
    void Update()
    {
        	if (firstPickup == false)
        {
            sanityManager.candleTimeLeft -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(objectGrabPointTransform.position);
        }
        
    }
}
