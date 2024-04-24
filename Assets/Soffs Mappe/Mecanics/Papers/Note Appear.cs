using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteAppear : MonoBehaviour
{
    [SerializeField] public Image noteImage;
    [SerializeField] public GameObject noteGameObject;
    [SerializeField] private GameObject noteCanvas; // Reference to the canvas GameObject

    public bool isVisible = false;

    void Start()
    {
        // Ensure the canvas is initially disabled
        noteCanvas.SetActive(false);
        noteImage.enabled = false;
        if (noteCanvas == null)
        {
            Debug.LogError("Note canvas not found");
        }
    }

    public void LookAtNote()
    {
        // Check if the player is looking at the note
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                // If the canvas and image are inactive, activate them
                if (!isVisible)
                {
                    noteCanvas.SetActive(true);
                    // Activate the corresponding image
                    if (noteImage != null)
                    {
                        noteImage.enabled = true;
                    }
                    isVisible = true;
                }
                
            }
        }
    }
    public void HideNote()
    {
        // Deactivate the canvas
        noteCanvas.SetActive(false);

        // Deactivate the corresponding image
        if (noteImage != null)
        {
            noteImage.enabled = false;
        }

        isVisible = false;
    }
}
