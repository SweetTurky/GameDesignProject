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
    private int totalNotes = 7; // Set to the total number of notes in your game
    public bool readyForPuzzle = false;
    public bool isGameWon = false;
    public bool isGameLost = false;
    public bool interact = false;
    public bool lanternHeld = false;
    public bool noteInteraction = false;
    public GameObject lanternInstruction;
    public GameObject instruction;
    public GameObject enableInteraction;
    public GameObject menuCanvas;
    public GameObject wordPuzzle;
    public FadeToBlack fadeToBlack;
    public GameObject firstPersonController;
    private HashSet<GameObject> collectedNotes = new HashSet<GameObject>();
    public NoteAppear[] noteAppearArray;
    public bool Document1Activated = false;
    public bool Document2Activated = false;
    public bool Document3Activated = false;
    public bool Document4Activated = false;
    public bool Document5Activated = false;
    public bool Document6Activated = false;
    public bool Document7Activated = false;
    public AudioListener audioListener;
    private ObjectGrabbable objectGrabbable;
    public TMP_Text notesTextField;
    public GameObject currentNote = null;

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

        // Find all GameObjects with the "Note" tag
        GameObject[] noteObjects = GameObject.FindGameObjectsWithTag("Note");

        // Loop through each note GameObject and add its NoteAppear script to the list
        foreach (GameObject noteObject in noteObjects)
        {
            NoteAppear noteAppear = noteObject.GetComponent<NoteAppear>();
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Note"))
        {
            instruction.SetActive(true);
            interact = true;
            noteInteraction = true;
            // Store the collided note for interaction
            currentNote = collision.gameObject;

        }
        else if (collision.transform.CompareTag("HandHeldLightSource"))
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
        currentNote = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Deactivate all document canvas and images
            foreach (NoteAppear document in noteAppearArray)
            {
                if (document.isVisible)
                {
                    document.HideNote();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && interact == true)
        {
            // Ensure there's a valid currentNote
            if (currentNote != null)
            {
                NoteAppear noteAppear = currentNote.GetComponent<NoteAppear>();
                if (noteAppear != null)
                {
                    noteAppear.LookAtNote();
                }
                // Check the name of the collided note
                string noteName = currentNote.name;

                // Check which document is being activated
                if (noteName == "Document1" && !Document1Activated)
                {
                    Document1Activated = true;
                    notesCollected++;
                }
                else if (noteName == "Document2" && !Document2Activated)
                {
                    Document2Activated = true;
                    notesCollected++;
                }
                else if (noteName == "Document3" && !Document3Activated)
                {
                    Document3Activated = true;
                    notesCollected++;
                }
                else if (noteName == "Document4" && !Document4Activated)
                {
                    Document4Activated = true;
                    notesCollected++;
                }
                else if (noteName == "Document5" && !Document5Activated)
                {
                    Document5Activated = true;
                    notesCollected++;
                }
                else if (noteName == "Document6" && !Document6Activated)
                {
                    Document6Activated = true;
                    notesCollected++;
                }
                else if (noteName == "Document7" && !Document7Activated)
                {
                    Document7Activated = true;
                    notesCollected++;
                }

                // Update the UI text field
                UpdateNotesTextField();

                // Remove the interaction after activation
                interact = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && objectGrabbable == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, pickUpDistance, pickUpLayerMask))
            {
                if (hit.transform.CompareTag("HandHeldLightSource"))
                {
                    lanternHeld = true;
                    interact = true; // Enable interaction
                    lanternInstruction.SetActive(false); // Show instruction
                    return; // Exit the method to avoid further checks
                }
            }
        }

        // Hide the instruction if the lantern is held
        if (lanternHeld)
        {
            lanternInstruction.SetActive(false);
        }
        if (notesCollected == totalNotes && readyForPuzzle == false)
        {
            wordPuzzle.SetActive(true);
            readyForPuzzle = true;
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
            FadeManager.instance.FadeToBlack();
            StartCoroutine(TurnDownAudioListener());
        }
    }

    public void WinGame()
    {
        Debug.Log("Congratulations! You win!");
        // Add any other win conditions or actions here
        SceneManager.LoadScene("OutroVideo");
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
    public IEnumerator TurnDownAudioListener()
    {
        float duration = 4.0f;
        float elapsedTime = 0f;
        float startVolume = AudioListener.volume;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            AudioListener.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        // Ensure volume is set to 0 when the coroutine ends
        AudioListener.volume = 0f;
    }
}