using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NoteRaycast : MonoBehaviour
{
    [Header ("Raycast Features")]
    [SerializeField] private float rayLength = 5;
    private Camera camera;

    private NoteController noteController;

    [Header("Crosshair and UI")]
    [SerializeField] private Image crosshair;
    [SerializeField] private GameObject interactionText;

    [Header("Input Key")]
    [SerializeField] private KeyCode interactKey;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update() 
    {
        if (Physics.Raycast(camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)), transform.forward, out RaycastHit hit, rayLength))
        {
            var readableItem = hit.collider.GetComponent<NoteController>();
            if (readableItem != null)
            {
                noteController = readableItem;
                HighLightCrosshair(true);
                interactionText.SetActive(true);
            }
            else
            {
                ClearNote();
            }
        }
        else 
        {
            ClearNote();
        }

        if(noteController != null)
        {
            if(Input.GetKeyDown(interactKey))
            {
                noteController.ShowNote();
            }

        }
    
    }

    void ClearNote()
    {
        if (noteController != null)
        {
            HighLightCrosshair(false);
            interactionText.SetActive(false);
            noteController = null;
        }
    }

    void HighLightCrosshair(bool on)
    {
        if (on)
        {
            crosshair.color = Color.red;
        }
        else
        {
            crosshair.color = Color.white;
        }
    }
}
