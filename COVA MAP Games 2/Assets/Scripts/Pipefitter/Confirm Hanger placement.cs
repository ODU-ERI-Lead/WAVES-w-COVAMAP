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


        //  foreach (GameObject hanger in hangerObjects)
        //  {
        //     hanger.SetActive(false);
        // }
        // Convert mouse position to world space
       // Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        // Calculate distance between mouse and target object
       // float distance = Vector3.Distance(mouseWorldPosition, hangerpositions.position);

        // Activate the current hanger in sequence
        hangerObjects[currentIndex].SetActive(true);

        Debug.Log("Activated Hanger: " + hangerObjects[currentIndex].name);

        // Move to the next one
        currentIndex++;


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
