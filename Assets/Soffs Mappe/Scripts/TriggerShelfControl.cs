using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShelfControl : MonoBehaviour
{
    [SerializeField] private Animator selfAnimator = null;
    [SerializeField] private AudioSource openSound = null; // Assign the open sound effect
    [SerializeField] private AudioSource closeSound = null; // Assign the close sound effect
    public float cooldownTime = 0.5f; // Adjust this value as needed
    public LayerMask layerMask; // Layer mask to detect only the drawer

    private bool isOpen = false; // Track whether the drawer is currently open or closed
    private float cooldownTimer = 0f; // Timer to track cooldown period

    private void Update()
    {
        // Check if the cooldown timer is active
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime; // Decrease cooldown timer
        }

        // Check if the left mouse button is being held down and cooldown is over
        if (Input.GetKeyDown(KeyCode.E) && cooldownTimer <= 0)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check if the ray hits something and that something is the drawer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (!isOpen)
                {
                    // Play the open animation
                    selfAnimator.Play("Open Shelf", 0, 0.0f);
                    isOpen = true; // Update the state to open
                    openSound.Play(); // Play the open sound effect
                }
                else
                {
                    // Play the close animation
                    selfAnimator.Play("Close Shelf", 0, 0.0f);
                    isOpen = false; // Update the state to closed
                    closeSound.Play(); // Play the close sound effect
                }

                cooldownTimer = cooldownTime; // Set cooldown timer
            }
        }
    }
}
