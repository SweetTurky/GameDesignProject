using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityManager : MonoBehaviour
{
    public float playerSanity = 100f;
    public float secondsSinceLightSource = 0f;
    public float sanityDecayRate = 1f;
    public float sanityIncreaseRate = 0.01f;
    public float raycastDistance = 10f; // Adjust this value according to your scene
    public LayerMask raycastLayerMask; // Layer mask to specify which layers the raycast should hit
    private bool nearLightSource = false;
    private int lastLoggedInterval = 100;
    public AudioSource audioSource;
    public float candleTimeLeft = 60f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LightSource"))
        {
            nearLightSource = true;
            Debug.Log("Near Light Source");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LightSource"))
        {
            nearLightSource = false;
            Debug.Log("Exiting Light Source");
        }
    }

    void Update()
    {
        // Perform a raycast from the player towards the light source
        RaycastHit hit;
        bool raycastHitSomething = Physics.Raycast(transform.position, (transform.position - transform.forward), out hit, raycastDistance, raycastLayerMask);

        if (nearLightSource && !raycastHitSomething)
        {
            secondsSinceLightSource = 0f;
            playerSanity += sanityIncreaseRate;
        }
        else
        {
            secondsSinceLightSource += Time.deltaTime; // This will increase the timer when not near a light source or obstructed by something.
        }

        // The code below updates player sanity based on time spent away from light sources
        playerSanity -= sanityDecayRate * Time.deltaTime;

        playerSanity = Mathf.Clamp(playerSanity, 0f, 100f); // this code clamps sanity within valid range from 0 to 100

        audioSource.volume = 1 - playerSanity / 100F;

        // Check sanity intervals and log messages accordingly
        if (playerSanity <= 100f && lastLoggedInterval > 100)
        {
            Debug.Log("Player Sanity between 100 - 80!");
            lastLoggedInterval = 100;
        }
        else if (playerSanity > 100f && lastLoggedInterval <= 100)
        {
            lastLoggedInterval = 101; // Reset when sanity goes above 100
        }
        else if (playerSanity <= 80f && lastLoggedInterval > 80)
        {
            Debug.Log("Player Sanity between 80 - 60!");
            lastLoggedInterval = 80;
        }
        else if (playerSanity > 80f && lastLoggedInterval <= 80)
        {
            lastLoggedInterval = 81; // Reset when sanity goes above 80
        }
        else if (playerSanity <= 60f && lastLoggedInterval > 60)
        {
            Debug.Log("Player Sanity between 60 - 40!");
            lastLoggedInterval = 60;
        }
        else if (playerSanity > 60f && lastLoggedInterval <= 60)
        {
            lastLoggedInterval = 61; // Reset when sanity goes above 60
        }
        else if (playerSanity <= 40f && lastLoggedInterval > 40)
        {
            Debug.Log("Player Sanity between 40 - 20!");
            lastLoggedInterval = 40;
        }
        else if (playerSanity > 40f && lastLoggedInterval <= 40)
        {
            lastLoggedInterval = 41; // Reset when sanity goes above 40
        }
        else if (playerSanity < 20f && lastLoggedInterval > 0)
        {
            Debug.Log("Player Sanity between 20 - 0!");
            lastLoggedInterval = 0;
        }
    }
}
