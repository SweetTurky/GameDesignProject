using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDrawerControl : MonoBehaviour
{
    [SerializeField] private Animator myDrawer = null;
    public float cooldownTime = 0.5f; // Adjust this value as needed

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
        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0)
        {
            if (!isOpen)
            {
                // Play the open animation
                myDrawer.Play("OpenDrawer", 0, 0.0f);
                isOpen = true; // Update the state to open
            }
            else
            {
                // Play the close animation
                myDrawer.Play("CloseDrawer", 0, 0.0f);
                isOpen = false; // Update the state to closed
            }

            cooldownTimer = cooldownTime; // Set cooldown timer
        }
    }
}

