using Unity.VisualScripting;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool hasBeenInteractedWith = false;

    public AudioSource audioSource;
    public AudioClip toTheChurchClip;

    public GameObject wordPuzzle;

    // This method is called when the object is interacted with
    public void Interact()
    {
        // Check if the object has been interacted with before
        if (!hasBeenInteractedWith)
        {
            // Increment your integer here
            IncrementNotesRead();

            // Set the flag to indicate that this object has been interacted with
            hasBeenInteractedWith = true;
        }
    }


private void IncrementNotesRead()
{
    // Increment the interaction count
    GameManager.instance.notesCollected++;
    GameManager.instance.UpdateNotesTextField();

    // Check if the interaction count has reached 7
    if (GameManager.instance.notesCollected == 7)
    {
        wordPuzzle.SetActive(true);
        audioSource.PlayOneShot(toTheChurchClip, 0.8f);
    }
    
}

}
