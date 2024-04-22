using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PuzzleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string word; // The word associated with this button
    public WordPuzzleManager puzzleManager;

    public AudioClip onHoverEnterClip;
    public AudioClip onHoverExitClip;

    public AudioClip onClickClip;
    public AudioSource audioSource;
    public Material hoverMaterial; // Material for hover state
    public Material pressedMaterial; // Material for pressed state
    public TMP_Text buttonText; // Text component of the button

    private Material normalMaterial; // Material for normal state

    public bool isPressed = false;

    void Start()
    {
        puzzleManager = FindObjectOfType<WordPuzzleManager>();
        normalMaterial = buttonText.fontSharedMaterial; // Store the original material
    }

    public void OnButtonClick()
    {
        if (!isPressed)
        {
        isPressed = true;
        // Add this button's word to the current order
        buttonText.fontSharedMaterial = pressedMaterial;
        puzzleManager.AddToCurrentOrder(word);
        AudioClip clip = onClickClip;
        audioSource.PlayOneShot(clip);
        Debug.Log("Button clicked");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPressed)
        {
            // Change material on hover
            buttonText.fontSharedMaterial = hoverMaterial;
            AudioClip clip = onHoverEnterClip;
            audioSource.PlayOneShot(clip);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPressed)
        {
        // Change material when not hovered
        buttonText.fontSharedMaterial = normalMaterial;
        //AudioClip clip = onHoverExitClip;
        //audioSource.PlayOneShot(clip);
        }
    }

    public void ResetMaterial()
    {
        buttonText.fontSharedMaterial = normalMaterial;
    }

}
