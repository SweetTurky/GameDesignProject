using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDrawerControl : MonoBehaviour
{
    [SerializeField] private Animator myDrawer = null;

    private bool isOpen = false; // Track whether the drawer is currently open or closed

    private void OnMouseDown()
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
    }
}
