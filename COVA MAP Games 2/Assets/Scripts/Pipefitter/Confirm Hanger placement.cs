using FuzzPhyte.Tools.Samples;
using FuzzPhyte.Tools;
using FuzzPhyte.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmHangerplacement : MonoBehaviour 
{

    public Button confirmButton;          
    public GameObject[] hangerObjects;
    public GameObject Promptpanel;
    protected int currentIndex = 0;
    public float activationDistance = 5f;
    public Transform[] hangerpositions;
    // void Start()
    // {
    //     confirmButton.onClick.AddListener(OnConfirmPressed);
    // }
   

    public void OnConfirmPressed()
    {
        if (currentIndex >= hangerObjects.Length)
        {
            Debug.Log("All hangers have already been activated.");
            Promptpanel.SetActive(false);
            return;
        }
        // Convert hanger world position to screen space
        //  Vector3 hangerScreenPos = Camera.main.WorldToScreenPoint(hangerpositions[currentIndex].position);

        // Get current mouse position (also in screen space)
        // Vector2 mouseScreenPos = Input.mousePosition;

        // Calculate distance in screen space (2D X/Y only)
        // float distance = Vector2.Distance(new Vector2(hangerScreenPos.x, hangerScreenPos.y), mouseScreenPos);
        // Get the depth of the hanger's position from the camera
        Vector3 hangerScreenPos = Camera.main.WorldToScreenPoint(hangerpositions[currentIndex].position);

        // Use the hanger's depth to convert the mouse position correctly
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, hangerScreenPos.z);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Calculate the distance from mouse to hanger
        float distance = Vector3.Distance(mouseWorldPosition, hangerpositions[currentIndex].position);
        Debug.Log($"Mouse to hanger distance: {distance}");

        Debug.Log($"Mouse distance to hanger {currentIndex}: {distance}");

        // Activate the current hanger in sequence
        if (distance <= activationDistance)
        {
            hangerObjects[currentIndex].SetActive(true);

        Debug.Log("Activated Hanger: " + hangerObjects[currentIndex].name);


        // Move to the next one
        currentIndex++;
        }
        else
        {
            Debug.Log("Click was too far from hanger. Hanger not activated.");
        }

        Promptpanel.SetActive(false);

        //temp debug method
        for (int i = 0; i < hangerObjects.Length; i++)
        {
            Debug.Log($"Index {i}: {hangerObjects[i]?.name}, Active: {hangerObjects[i]?.activeSelf}");
        }

        Promptpanel.SetActive(false);
    }
   
    public void NoConfirmPressed()
    { 
        Promptpanel.SetActive(false);
    
    }


}
