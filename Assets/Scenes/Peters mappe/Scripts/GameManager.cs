using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public int notesCollected = 0;
    public int totalNotes = 5; // Set to the total number of notes in your game

    public bool isGameWon = false;
    public bool isGameLost = false;
    public bool interact = false;
    public bool lanternHeld = false;
    public bool noteInteraction = false;
    public GameObject lanternInstruction;
    public GameObject instruction;
    public GameObject enableInteraction;
    public GameObject menuCanvas;
    public FadeToBlack fadeToBlack;
    public GameObject firstPersonController;
    public NoteAppear noteAppear;
    private HashSet<GameObject> collectedNotes = new HashSet<GameObject>();
    public AudioListener audioListener;
    private ObjectGrabbable objectGrabbable;
    public TMP_Text notesTextField;

    public float pickUpDistance = 2f; // Distance for raycasting to pick up objects
    public LayerMask pickUpLayerMask; // Layer mask for objects that can be picked up
    public Transform playerCameraTransform; // Camera transform to use for raycasting



    private void Awake()
    {
        
        // Ensure only one instance of the GameManager exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        instruction.SetActive(false);
        lanternInstruction.SetActive(false);
        enableInteraction.SetActive(true);
        UpdateNotesTextField();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Note"))
        {
            instruction.SetActive(true);
            interact = true;
            noteInteraction = true;

        }
        else if (collision.transform.CompareTag("Lantern"))
        {
            lanternInstruction.SetActive(true);
            interact = true;
        }
        else if (collision.transform.CompareTag("Cabinet") || collision.transform.CompareTag("Furniture"))
        {
            instruction.SetActive(true);
            interact = true;
        }
        else
        {
            instruction.SetActive(false);
            lanternInstruction.SetActive(false);
        }
    
    }

    void OnTriggerExit(Collider collision)
    {
        instruction.SetActive(false);
        lanternInstruction.SetActive(false);
        interact = false;
        noteInteraction = false;
    }

    void Update()
    {
            if (Input.GetKeyDown(KeyCode.E) && interact == true)
    {
        // If the canvas is already visible, hide it
        if (noteAppear.isVisible)
        {
            noteAppear.LookAtNote();
            interact = false; // Ensure interaction is disabled after putting down the note
            return;
        }

        // If the player is in range to interact with a note and the note is not already collected
        if (noteInteraction == true && !noteAppear.isVisible)
        {
            instruction.SetActive(false);
            enableInteraction.SetActive(false);
            interact = false;

            // Check if the collided object is a note and hasn't been collected yet
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Note") && !collectedNotes.Contains(collider.gameObject))
                {
                    CollectNote();
                    noteAppear.LookAtNote();
                    collectedNotes.Add(collider.gameObject);
                    interact = true;
                    break; // Exit loop after collecting one note
                }
            }
        }
    }

        /*else if (noteAppear.isVisible && Input.GetKeyDown(KeyCode.E))
            {
                noteAppear.LookAtNote();
            }*/

        if (Input.GetKeyDown(KeyCode.Mouse0) && objectGrabbable == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, pickUpDistance, pickUpLayerMask))
            {
                if (hit.transform.CompareTag("Lantern"))
                {
                    interact = true; // Enable interaction
                    lanternInstruction.SetActive(true); // Show instruction
                    return; // Exit the method to avoid further checks
                }
            }
        }

        // Hide the instruction if the lantern is held
        if (lanternHeld)
        {
            lanternInstruction.SetActive(false);
        }
    }      


    public void CollectNote()
    {
        if (noteAppear.isVisible)
        {
            // Check if the note has already been collected
            if (!collectedNotes.Contains(noteAppear.noteGameObject))
            {
                // Increment the total number of notes collected
                notesCollected++;
                Debug.Log("Note collected! Total notes: " + notesCollected);
                UpdateNotesTextField();

                // Add the note to the set of collected notes
                collectedNotes.Add(noteAppear.noteGameObject);

                // Check if all notes have been collected
                if (notesCollected >= totalNotes)
                {
                    WinGame();
                }
            }

            // Hide the note canvas
            noteAppear.HideNote();
        }
    }


    void UpdateNotesTextField()
    {
        if (notesTextField != null)
        {
            notesTextField.text = "Notes: " + notesCollected + " / " + totalNotes;
            Debug.Log("Notes: " + notesCollected + " / " + totalNotes);
        }
        else
        {
            Debug.LogWarning("UI Text field for notes count is not assigned!");
        }
    }

    public void LoseGame()
    {
        if (isGameLost == true)
        {
            menuCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            fadeToBlack.StartFade();
            StartCoroutine(TurnOffAudioListener());
        }
    }

    public void WinGame()
    {
        isGameWon = true;
        if (isGameWon == true)
        {
            Debug.Log("Congratulations! You win!");
            // Add any other win conditions or actions here
        }
    }

    public void LoadNextLevel()
    {
        // Load the next level (assuming levels are organized in order)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartGame()
    {
        // Restart the current level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        // Quit the game (works in standalone builds)
        Application.Quit();
    }
    IEnumerator TurnOffAudioListener()
    {       
        yield return new WaitForSeconds(4.0f);
            if (audioListener != null)
            {
                audioListener.enabled = false;
            }
            else
            {
                Debug.LogWarning("AudioListener component not found.");
            }
            
        
    }
}