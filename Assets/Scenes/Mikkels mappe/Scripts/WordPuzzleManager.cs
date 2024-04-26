using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WordPuzzleManager : MonoBehaviour
{
    public List<string> solutionOrder; // The correct order of button clicks
    private List<string> currentOrder; // The current order of button clicks

    public AudioClip incorrectSolutionClip;

    public AudioClip correctSolutionClip;
    public AudioSource audioSource;

    public Animator animator;

    public Animator animator2;

    void Start()
    {
        currentOrder = new List<string>();
    }

    public void AddToCurrentOrder(string word)
    {
        currentOrder.Add(word);

        // Check if the current order matches the solution order
        if (CheckSolution())
        {
            Debug.Log("Puzzle Solved!");
            StartCoroutine("PuzzleSolved");
        }
    }

    private bool CheckSolution()
    {
        if (currentOrder.Count != solutionOrder.Count)
        {
            return false;
        }

        for (int i = 0; i < solutionOrder.Count; i++)
        {
            if (solutionOrder[i] != currentOrder[i])
            {
                // Play sound for incorrect solution
                AudioClip clip = incorrectSolutionClip;
                audioSource.PlayOneShot(clip);
                Debug.Log("Incorrect solution!");
                animator.Play("Taunt");
                animator2.Play("Taunt");
                // Reset puzzle buttons
                ResetPuzzleButtons();               
                // Reset current order
                currentOrder.Clear();
                return false;
            }
        }

        return true;
    }

    public IEnumerator PuzzleSolved()
    {
        // Implement actions to take when the puzzle is solved
        AudioClip clip = correctSolutionClip;
        audioSource.PlayOneShot(clip, 2f);
        Debug.Log("Congratulations! Puzzle solved!");
        animator.Play("Death");
        animator2.Play("Death");
        yield return new WaitForSeconds(4f);
        FadeManager.instance.FadeToBlack();
        GameManager.instance.TurnDownAudioListener();
        yield return new WaitForSeconds(3f);
        GameManager.instance.WinGame();
    }

    private void ResetPuzzleButtons()
    {
        // Find all PuzzleButton objects in the scene
        PuzzleButton[] puzzleButtons = FindObjectsOfType<PuzzleButton>();
        foreach (PuzzleButton puzzleButton in puzzleButtons)
        {
            // Reset IsPressed flag to false
            puzzleButton.isPressed = false;
            // Set material to normal material
            puzzleButton.ResetMaterial();
        }
    }
}
