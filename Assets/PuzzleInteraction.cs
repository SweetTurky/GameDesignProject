using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzleInteraction : MonoBehaviour
{
    public Transform playerCamera; // Reference to the player's camera
    public LayerMask buttonLayerMask; // Layer mask for puzzle buttons
    public WordPuzzleManager wordPuzzleManager;

    private GameObject lastHoveredObject; // Track the last UI element hovered over

    void Update()
    {
        RaycastHit hit;

        // Cast a ray from the center of the screen (crosshair position)
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 20f, buttonLayerMask))
        {
            // Check if the object hit is a puzzle button
            PuzzleButton puzzleButton = hit.collider.GetComponent<PuzzleButton>();
            if (puzzleButton != null)
            {
                // If it is a puzzle button, handle the hover effect
                HandleHover(puzzleButton.gameObject);
            }
        }
        else
        {
            // If not hovering over any UI element, trigger exit event for previously hovered element
            if (lastHoveredObject != null)
            {
                HandleExit(lastHoveredObject);
                lastHoveredObject = null;
            }
        }

        // Handle click event
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (lastHoveredObject != null)
            {
                // Check if the last hovered object is a puzzle button
                PuzzleButton puzzleButton = lastHoveredObject.GetComponent<PuzzleButton>();
                if (puzzleButton != null)
                {
                    // If it is a puzzle button, handle the button click
                    puzzleButton.OnButtonClick();
                }
            }
        }
    }

    // Handle hover effect
    void HandleHover(GameObject hoveredObject)
    {
        if (hoveredObject != lastHoveredObject)
        {
            // Trigger exit event for previously hovered element
            if (lastHoveredObject != null)
            {
                HandleExit(lastHoveredObject);
            }

            // Trigger enter event for current hovered element
            ExecuteEvents.Execute(hoveredObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
            lastHoveredObject = hoveredObject;
        }
    }

    // Handle exit event
    void HandleExit(GameObject exitedObject)
    {
        ExecuteEvents.Execute(exitedObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
    }
}
