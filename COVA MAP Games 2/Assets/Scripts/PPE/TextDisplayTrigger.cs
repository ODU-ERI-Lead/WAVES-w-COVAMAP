using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI.text;
//using UnityEngine.TextMeshPro;
using TMPro;
using System.Collections;

public class TextDisplayTrigger : MonoBehaviour
 {
    public TextMeshProUGUI[] textAssets;  // Array of TextMeshPro or Text components
    public Image[] imageAssets;          // Array of Image components
    public float displayDuration = 1f;    // Duration for each text to be displayed
    public float delayBetweenText = 0.2f; // Delay between each text display

    private int currentTextIndex = 0;     // Track which text is being displayed

    private void Start()
    {
        // Ensure all text and images are hidden at the start
        foreach (var text in textAssets)
        {
            text.gameObject.SetActive(false);
        }

        foreach (var image in imageAssets)
        {
            image.gameObject.SetActive(false);
        }

        // Start the display sequence as soon as the scene is loaded
        StartCoroutine(DisplayTextSequence());
    }

    private IEnumerator DisplayTextSequence()
    {
        while (currentTextIndex < textAssets.Length)
        {
            // Display the current text
            textAssets[currentTextIndex].gameObject.SetActive(true);

            // Display the corresponding image (if exists)
            if (currentTextIndex < imageAssets.Length)
            {
                imageAssets[currentTextIndex].gameObject.SetActive(true);
            }

            // Wait for the duration before hiding the text and image
            yield return new WaitForSeconds(displayDuration);

            // Hide the current text
            textAssets[currentTextIndex].gameObject.SetActive(false);

            // Hide the image (if it exists)
            if (currentTextIndex < imageAssets.Length)
            {
                imageAssets[currentTextIndex].gameObject.SetActive(false);
            }

            // Move to the next text and wait before showing it
            currentTextIndex++;
            yield return new WaitForSeconds(delayBetweenText);
        }

        // Optionally reset or stop the sequence if needed
    }
}
