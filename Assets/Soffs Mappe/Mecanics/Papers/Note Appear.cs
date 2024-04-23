using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteAppear : MonoBehaviour
{
    [SerializeField]
    public Image noteImage;
    public GameObject noteGameObject;
    public bool isVisible = false;

    void Start()
    {
        // Ensure the document is initially hidden
        noteImage.enabled = false;
    }

    public void LookAtNote()
    {
        // Toggle visibility of the note image
        if (!isVisible)
        {
            // Note is not visible, make it visible
            noteImage.enabled = true;
            isVisible = true; // Update the isVisible variable
            noteGameObject.SetActive(false);
        }
        else
        {
            // Note is visible, make it invisible
            HideNote();
        }
    }

    public void HideNote()
        {
            // Hide the note image and make it invisible
            noteImage.enabled = false;
            isVisible = false; // Update the isVisible variable
            noteGameObject.SetActive(true);
        }

}