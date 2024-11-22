using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI.text;
//using UnityEngine.TextMeshPro;
using TMPro;
using System.Collections;

public class TextDisplayTrigger : MonoBehaviour
 {
    public TextMeshProUGUI[] textAssets; // Array of TextMeshPro or Text components
    public float displayDuration = 1f;   // Duration for each text to be displayed
    public float delayBetweenText = 0.2f; // Delay between each text display

    private bool isTriggered = false; // Whether the collider has been triggered
    private int currentTextIndex = 0; // Track which text is being displayed

    private void OnTriggerEnter(Collider other)
    {
        // Check if the triggering object has a specific tag or condition
        if (other.CompareTag("Player")) // Modify with your tag or condition
        {
            isTriggered = true;
            currentTextIndex = 0;
            StartCoroutine(DisplayTextSequence());
        }
    }

    private IEnumerator DisplayTextSequence()
    {
        while (isTriggered && currentTextIndex < textAssets.Length)
        {
            // Display the current text
            textAssets[currentTextIndex].gameObject.SetActive(true);
            
            // Wait for the duration before hiding it
            yield return new WaitForSeconds(displayDuration);

            // Hide the current text
            textAssets[currentTextIndex].gameObject.SetActive(false);

            // Move to the next text and wait before showing it
            currentTextIndex++;
            yield return new WaitForSeconds(delayBetweenText);
        }

        // Optionally, reset or stop the sequence if needed
        isTriggered = false;
    }
}
