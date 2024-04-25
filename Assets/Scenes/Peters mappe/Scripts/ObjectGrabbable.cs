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
    private bool isFading = false; // Flag to track fading
    public float lerpSpeed = 30f;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        lanternLight = GetComponentInChildren<Light>();
        initialIntensity = lanternLight.intensity;
        //lightTimer = sanityManager.candleTimeLeft;
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        GameManager.instance.lanternInstruction.SetActive(false); // Disable instruction when the lantern is grabbed

        if (firstPickup)
        {
            firstPickup = false;
            sanityManager.candleTimeLeft = 60f; // Set the timer to 60 seconds
            StartCoroutine(UpdateLightTimer()); // Start updating the light timer
        }
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        GameManager.instance.lanternHeld = false;

        // Show instruction when the lantern is dropped and player is looking at it
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, GameManager.instance.pickUpDistance, GameManager.instance.pickUpLayerMask))
        {
            if (hit.transform.CompareTag("HandHeldLightSource"))
            {
                GameManager.instance.lanternInstruction.SetActive(true);
            }
        }
    }

    public void IncreaseLightTimer()
    {
        if (sanityManager.candleTimeLeft < 60f)
        {
            sanityManager.candleTimeLeft += lightTimerIncreaseAmount;
            //lightTimer = sanityManager.candleTimeLeft;
            RestoreLight(); // Restore light intensity after increasing the timer
            // Stop any ongoing fading coroutine
            StopCoroutine("FadeLightIntensity");
            // Start fading the light intensity only if necessary
            StartCoroutine(FadeLightIntensity());
            
        }
    }

    IEnumerator UpdateLightTimer()
    {
        while (sanityManager.candleTimeLeft > 0f)
        {
            if (!isFading && sanityManager.candleTimeLeft <= 5f && sanityManager.candleTimeLeft > 0f) // Check if fading should start
            {
                isFading = true;
                StartCoroutine(FadeLightIntensity()); // Start fading the light intensity
            }

            //sanityManager.candleTimeLeft -= Time.deltaTime; // Update light timer
            yield return null;
        }
    }

    IEnumerator FadeLightIntensity()
    {
        float elapsedTime = 0f;
        // Only start fading if lightTimer is 5 or under
        if (sanityManager.candleTimeLeft <= 5f)
        {
            isFading = true;
            while (elapsedTime < 5f) // Fading duration is 5 seconds
            {
                float progress = elapsedTime / 5f; // Calculate progress
                float newIntensity = Mathf.Lerp(initialIntensity, 0f, progress); // Calculate new intensity
                lanternLight.intensity = newIntensity; // Set light intensity
                elapsedTime += Time.deltaTime; // Update elapsed time
                yield return null;
            }

            // Ensure light intensity is set to 0
            lanternLight.intensity = 0f;
            // Reset fading flag
            isFading = false;
        }
    }

    void RestoreLight()
    {
        // Restore light intensity
        lanternLight.intensity = initialIntensity;
    }

    void Update()
    {
        if (!firstPickup)
        {
            sanityManager.candleTimeLeft -= Time.deltaTime;
        }
        if (sanityManager.candleTimeLeft <= 0)
        {
            // Ensure light is off when timer reaches 0
            lanternLight.intensity = 0f;
        }
        if (objectGrabPointTransform != null)
        {
            // Move the object towards the grab point
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(objectGrabPointTransform.position);
        }
    }
}
