using UnityEngine;

public class AnswerIcon : MonoBehaviour
{
     public GameObject correctSprite; // Drag the "Correct" sprite here
    public GameObject incorrectSprite; // Drag the "Incorrect" sprite here

    // Call this method with the answer's correctness
    public void ShowAnswerIcon(bool isCorrect)
    {
        // Deactivate both sprites first
        correctSprite.SetActive(false);
        incorrectSprite.SetActive(false);

        // Activate the appropriate sprite based on the answer
        if (isCorrect)
        {
            correctSprite.SetActive(true);
        }
        else
        {
            incorrectSprite.SetActive(true);
        }

        // Optionally, hide the feedback after a delay
        Invoke("HideFeedback", 2f); // Hide after 2 seconds
    }
    // Code needed for activation based on different scenarios

    // Hide both sprites
    private void HideFeedback()
    {
        correctSprite.SetActive(false);
        incorrectSprite.SetActive(false);
    }
}
