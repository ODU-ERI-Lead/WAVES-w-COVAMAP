using FuzzPhyte.Tools.Samples;
using UnityEngine;
using System.Collections.Generic;
using FuzzPhyte.Game.Samples;
using UnityEngine.Events;

public class ConfirmHangerplacement : MonoBehaviour 
{
    [Tooltip("Main Hanger Objects in scene to be turned on/off")]
    public List<GameObject> AllHangerObjects = new List<GameObject>();
    protected int numberCorrect = 0;
    public float activationDistance = 5f;
    public FP_MeasureTool3D MeasureToolRef;
    public PFHangerPlacementTool HangerToolRef;
    [Tooltip("This is an invisible object")]
    public GameObject ShellHangerItem;
    [Tooltip("If we want to clear measurements when the correct hanger is placed, set this to true.")]
    public bool ClearMeasurementsIfCorrectHanger = false;
    public FPGameManager_ToolExample GameManagerToolRef;
    public GameObject Tool3DReferenceObject;
    public GameObject HangerReferenceObject;
    public UnityEvent AllHangersConfirmedFireOnce;
    public void OnEnable()
    {
        if (MeasureToolRef != null)
        {
            //MeasureToolRef.OnMeasureToolEnding.AddListener(ActivatePromptPanel);
        }
        if (HangerToolRef != null)
        {
            HangerToolRef.OnHangerToolClickedRelease.AddListener(HangerConfirmSkip);
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
            //MeasureToolRef.OnMeasureToolEnding.RemoveListener(ActivatePromptPanel);
        }
        if (HangerToolRef != null)
        {
            HangerToolRef.OnHangerToolClickedRelease.RemoveListener(HangerConfirmSkip);
        }
    }
    public void HangerConfirmSkip()
    {
        OnConfirmPressed();
    }
    #if UNITY_EDITOR
    [ContextMenu("Spawn all Hangers Now")]
    public void OverrideHangerPlacement()
    {
        for(int i=0; i < AllHangerObjects.Count; i++)
        {
            if (AllHangerObjects[i] != null)
            {
                AllHangerObjects[i].SetActive(true);
            }
        }
        AllHangerObjects.Clear();
        HangersInPlace();
    }
    #endif
    public void OnConfirmPressed()
    {
        if (AllHangerObjects.Count == 0 || MeasureToolRef == null || HangerToolRef == null)
        {
            Debug.Log("All hangers have already been activated.");
            //Promptpanel.SetActive(false);
            //if we are in the measurement phase we want to activate all of the other buttons
            if (GameManagerToolRef.ReturnGameState == PipeFitterGameState.Measurements)
            {
                GameManagerToolRef.UpdatePipeFitterState(PipeFitterGameState.Parts);
            }
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
        //Vector3 endMeasuredPosition = MeasureToolRef.EndPosition;
        Vector3 endHangerPosition = HangerToolRef.ReturnLastEndPos;
        GameObject FoundHanger = null;
        for (int i = 0; i < AllHangerObjects.Count; i++)
        {
            float distance = Vector3.Distance(endHangerPosition, AllHangerObjects[i].transform.position);
            // Calculate the distance from mouse to hanger

            Debug.Log($"Measurement End to hanger distance: {distance}");

            Debug.Log($"Measurement End distance to hanger {AllHangerObjects[i].name}: {distance}");

            // Activate the current hanger in sequence
            if (distance <= activationDistance)
            {
                AllHangerObjects[i].SetActive(true);
                if (ShellHangerItem != null)
                {
                    if (ShellHangerItem.GetComponent<PFHangerShell>())
                    {
                        ShellHangerItem.GetComponent<PFHangerShell>().HangerPlaced(AllHangerObjects[i].transform.position);
                    }
                }
                
                Debug.Log("Activated Hanger: " + AllHangerObjects[i].name);
                FoundHanger = AllHangerObjects[i];
                if (ClearMeasurementsIfCorrectHanger && GameManagerToolRef != null && Tool3DReferenceObject != null)
                {
                    GameManagerToolRef.UI3DToolRemoveLines(Tool3DReferenceObject);
                }
                if (ClearMeasurementsIfCorrectHanger && GameManagerToolRef != null && HangerReferenceObject != null)
                {
                    GameManagerToolRef.UIRemoveHangerLines(HangerReferenceObject);
                }
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
        HangersInPlace();
    }
    
    private void HangersInPlace()
    {
        if (AllHangerObjects.Count == 0)
        {
            if (MeasureToolRef != null)
            {
                MeasureToolRef.OnMeasureToolEnding.RemoveListener(ActivatePromptPanel);
            }
            //if we are in the measurement phase we want to activate all of the other buttons
            if (GameManagerToolRef.ReturnGameState == PipeFitterGameState.Measurements)
            {
                GameManagerToolRef.UpdatePipeFitterState(PipeFitterGameState.Parts);
            }
            AllHangersConfirmedFireOnce.Invoke();
            GameManagerToolRef.OnMeasurementComplete.Invoke();
        }
    }
   // public UnityEvent AllHangersInPlace()
    // {

    // }
    // public delegate event AllHangersInPlace();
    public void NoConfirmPressed()
    {
        //Promptpanel.SetActive(false);

    }
    public void ActivatePromptPanel()
    {
        //Promptpanel.SetActive(true);
    }
}
