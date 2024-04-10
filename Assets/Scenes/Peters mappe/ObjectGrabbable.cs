using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private bool firstPickup = true;
    private float lightTimer = 0f;
    public Light lanternLight;
    public SanityManager sanityManager;
    public float maxRange = 10f;
    private float initalRange;
    private float refillTime = 30f;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        lanternLight = GetComponentInChildren<Light>();
        lightTimer = sanityManager.candleTimeLeft; 
        initalRange = playerLight
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

    private IEnumerator FadeLightIntensity()
    {
        float startIntensity = lanternLight.intensity;
        float fadeDuration = lightTimer; // Fade duration equals the remaining time on the timer

        // Gradually decrease light intensity to 0 over fadeDuration seconds
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            // Calculate the progress based on the elapsed time
            float progress = elapsedTime / fadeDuration;

            // Calculate the new intensity based on the progress
            float newIntensity = Mathf.Lerp(startIntensity, 0f, progress);

            // Set the light intensity
            lanternLight.intensity = newIntensity;

            // Update the candle time left in the SanityManager
            sanityManager.candleTimeLeft -= Time.deltaTime;

            // Wait for the next frame
            yield return null;

            // Update the elapsed time
            elapsedTime += Time.deltaTime;
        }

        // Ensure light intensity is set to 0
        lanternLight.intensity = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Refill the lantern if the player picks up a refill object (assuming the object has a "Refill" tag)
        if (other.CompareTag("Refill"))
        {
            lightTimer += 30f; // Add 30 seconds to the timer
            if (lightTimer > 60f) // Limit timer to maximum of 60 seconds
            {
                lightTimer = 60f;
            }
            Destroy(other.gameObject); // Destroy the refill object
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
