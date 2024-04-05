using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSkabHøjre : MonoBehaviour
{
    [SerializeField] private Animator myClosetLeft = null;

    private bool isOpen = false; // Track whether the drawer is currently open or closed

    private void OnMouseDown()
    {
        if (!isOpen)
        {
            // Play the open animation
            myClosetLeft.Play("Skabsdør Højre", 0, 0.0f);
            isOpen = true; // Update the state to open
        }
        else
        {
            // Play the close animation
            myClosetLeft.Play("Skabsdør Højre Luk", 0, 0.0f);
            isOpen = false; // Update the state to closed
        }
    }
}
