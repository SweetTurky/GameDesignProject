using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public int notesCollected = 0;
    public int totalNotes = 5; // Set to the total number of notes in your game

    public bool isGameWon = false;
    public bool isGameLost = false;
    public bool interact = false;
    public bool lanternHeld = false;
    public GameObject instruction;
    public GameObject enableInteraction;
    public GameObject objectOnGround;
    public GameObject objectOnHand;
    public Transform playerHandTransform;
    private Vector3 originalLanternPosition; // Store the original position of the lantern
    private Quaternion originalLanternRotation; // Store the original rotation of the lantern


    private HashSet<GameObject> collectedNotes = new HashSet<GameObject>();

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
        enableInteraction.SetActive(true);
        objectOnGround.SetActive(true);
        objectOnHand.SetActive(false);
        originalLanternPosition = objectOnGround.transform.position; // Store the original position of the lantern
        originalLanternRotation = objectOnGround.transform.rotation; // Store the original rotation of the lantern

    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Note") || collision.transform.CompareTag("Lantern") || collision.transform.CompareTag("Cabinet") || collision.transform.CompareTag("Furniture"))
        {
            instruction.SetActive(true);
            interact = true;
        }
        else
        {
            instruction.SetActive(false);
        }
    
    }

    void OnTriggerExit(Collider collision)
    {
        instruction.SetActive(false);
        interact = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interact == true)
            {
                instruction.SetActive(false);
                objectOnGround.SetActive(false);
                objectOnHand.SetActive(true);
                enableInteraction.SetActive(false);
                interact = false;

                // Check if the collided object is a note and hasn't been collected yet
                Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Note") && !collectedNotes.Contains(collider.gameObject))
                    {
                        CollectNote();
                        collectedNotes.Add(collider.gameObject);
                        break; // Exit loop after collecting one note
                    }
                    else if (collider.CompareTag("Lantern"))
                    {
                        GrabOrLeaveLantern(); // Call GrabOrLeaveLantern method
                    }
                }
            }
        }
    }

    public void CollectNote()
    {
        notesCollected++;
        Debug.Log("Note collected! Total notes: " + notesCollected);

        // Check win condition
        if (notesCollected >= totalNotes)
        {
            WinGame();
        }
    }
    public void GrabOrLeaveLantern()
{
    if (!lanternHeld)
    {
            // Pick up the lantern
            lanternHeld = true;
            objectOnHand.SetActive(true);  // Show the lantern in the player's hand
            objectOnGround.SetActive(false);  // Hide the lantern on the ground
            // Attach the lantern to the player's hand or parent it to the player's hand GameObject
            objectOnGround.transform.SetParent(playerHandTransform);  // Assuming playerHandTransform is the transform of the player's hand
            // Reset the local position and rotation of the lantern to its original values
            objectOnGround.transform.localPosition = Vector3.zero; // Place the lantern at the origin of the player's hand
            objectOnGround.transform.localRotation = Quaternion.identity; // Reset the rotation of the lantern
        }
        else
        {
            // Drop the lantern
            lanternHeld = false;
            objectOnHand.SetActive(false);  // Hide the lantern in the player's hand
            objectOnGround.SetActive(true);  // Show the lantern on the ground
            // Detach the lantern from the player's hand
            objectOnGround.transform.SetParent(null);
            // Set the position of the lantern directly below the player's hand
            objectOnGround.transform.position = playerHandTransform.position - playerHandTransform.up; // Place the lantern one unit below the player's hand position
        }
    }


    public void LoseGame()
    {
        if (isGameLost == true)
        {
            Debug.Log("Game Over! You lose.");
            // Add any other lose conditions or actions here
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
}
