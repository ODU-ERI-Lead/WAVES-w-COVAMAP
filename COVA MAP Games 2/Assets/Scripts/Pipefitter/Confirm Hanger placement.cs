using FuzzPhyte.Tools.Samples;
using FuzzPhyte.Tools;
using FuzzPhyte.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ConfirmHangerplacement : MonoBehaviour 
{

    public Button confirmButton;          
    //public GameObject[] hangerObjects;
    public List<GameObject> AllHangerObjects = new List<GameObject>();
    public GameObject Promptpanel;
    protected int numberCorrect = 0;
    public float activationDistance = 5f;
    public FP_MeasureTool3D MeasureToolRef;
    public UnityEvent AllHangersInPlace;

    public void OnEnable()
    {
        if (MeasureToolRef != null)
        {
            MeasureToolRef.OnMeasureToolEnding.AddListener(ActivatePromptPanel);
        }
    }
    public void Start()
    {
        for (int i = 0; i < AllHangerObjects.Count; i++)
        {
            AllHangerObjects[i].SetActive(false);
        }
    }
    public void OnDisable()
    {
        if (MeasureToolRef != null)
        {
            MeasureToolRef.OnMeasureToolEnding.RemoveListener(ActivatePromptPanel);
        }
    }
    public void OnConfirmPressed()
    {
        if(AllHangerObjects.Count == 0||MeasureToolRef==null)
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
        //Vector3 hangerScreenPos = Camera.main.WorldToScreenPoint(hangerObjects[currentIndex].transform.position);

        // Use the hanger's depth to convert the mouse position correctly
        //Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, hangerScreenPos.z);
        //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector3 endMeasuredPosition = MeasureToolRef.EndPosition;
        GameObject FoundHanger = null;
        for(int i=0;i< AllHangerObjects.Count; i++)
        {
            float distance = Vector3.Distance(endMeasuredPosition, AllHangerObjects[i].transform.position);
            // Calculate the distance from mouse to hanger

            Debug.Log($"Measurement End to hanger distance: {distance}");

            Debug.Log($"Measurement End distance to hanger {AllHangerObjects[i].name}: {distance}");

            // Activate the current hanger in sequence
            if (distance <= activationDistance)
            {
                AllHangerObjects[i].SetActive(true);

                Debug.Log("Activated Hanger: " + AllHangerObjects[i].name);
                FoundHanger=AllHangerObjects[i];

                // Move to the next one
                numberCorrect++;
                break;
            }
            else
            {
                Debug.Log("Click was too far from hanger. Hanger not activated.");
            }
        }

        AllHangerObjects.Remove(FoundHanger);
        if (AllHangerObjects.Count == 0)
        {
            if (MeasureToolRef != null)
            {
                MeasureToolRef.OnMeasureToolEnding.RemoveListener(ActivatePromptPanel);
            }
            AllHangersInPlace.Invoke();
        }
        Promptpanel.SetActive(false);
    }
   
   // public delegate event AllHangersInPlace();
    public void NoConfirmPressed()
    { 
        Promptpanel.SetActive(false);
    
    }
    public void ActivatePromptPanel()
    {
        Promptpanel.SetActive(true);
    }
}
