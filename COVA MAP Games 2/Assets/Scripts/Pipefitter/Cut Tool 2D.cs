
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using FuzzPhyte.Utility;
using UnityEngine.UI;
using FuzzPhyte.Tools;
using Unity.VisualScripting;
using FuzzPhyte.Tools.Samples;
using System;


[RequireComponent(typeof(RectTransform))]


public class CutTool2D : FP_Tool<FP_CutToolData>, IFPUIEventListener<FP_Tool<FP_MeasureToolData>> ///IPointerDownHandler, IPointerUpHandler,
{
    [Space]
    [Header("keep Using the Cut Tool?")]
    public bool ConstantCutmeasurement = false;
    [Space]
    [SerializeField] protected Canvas canvasRect;
    protected CanvasScaler canvasScaler;
    [Tooltip("Where do we want our Cut measure lines to be saved or maybe shown undeer")]
    [SerializeField] protected RectTransform CutParentSpace;
    [Header("Unity Events")]
    public UnityEvent OnCutToolActivation;
    public UnityEvent OnCutToolStarting;
    public UnityEvent OnCutToolActiveUse;
    public UnityEvent OnCutToolEnding;
    public UnityEvent OnCutToolDeactivation;
    [Header("Internal Parameters")]
    protected Vector2 CutstartPosition = Vector2.zero;
    protected Vector2 CutendPosition = Vector2.zero;
    [SerializeField] protected FP_CutLine_AC currentActiveLine;
    [SerializeField] protected List<FP_CutLine_AC> allCutLines = new List<FP_CutLine_AC>();
    protected int lineSortOrderCounter = 10;
    

    [Header("URP Shader Graph stuff")]
   [SerializeField] protected Material URP_Shadergraph_Cutmat;
    protected float Cut_per = 1f;//may need to change but this is in reference to the shader materials float of "cut per" not sure if I leave blank or give it its assigned value here?
    public string Cutmat = "Cut_per";
    public Renderer CutmatRenderer; 


    //Think this concludes all need prereq info/values             new(URP_Shadergraph_Cutmat) 


    /// <summary>
    /// Public accessor from UI to start the "tool"
    /// </summary>
    public void ButtonUI()
    {
        Initialize(toolData);
        ActivateTool();
    }

    public void DeactivateResetLinesUI()
    {
        ForceDeactivateTool();
        //blast all the lines
        foreach (var line in allCutLines)
        {
            Destroy(line.gameObject);
        }
        allCutLines.Clear();
    }
    public override void DeactivateToolFromUI()
    {
        DeactivateTool();
    }

    /// <summary>
    /// setting our state up for being ready to use the tool
    /// </summary>
    public override bool ActivateTool()
    {
        if (base.ActivateTool())
        {
            ToolIsCurrent =true;
            OnCutToolActivation.Invoke();
            return true;
        }
        Debug.LogWarning($"Did not activate the tool?");
        return false;
    }

    public void OnUIEvent(FP_UIEventData<FP_Tool<FP_MeasureToolData>> eventData)
    {
        Debug.LogWarning($"OnUIEvent was processed {eventData.EventType} {eventData.AdditionalData} {this} {ToolIsCurrent}");
        if (!ToolIsCurrent)
        {
            return;
        }
        if (eventData.TargetObject == this.gameObject)
        {

            //it's me
            //Debug.LogWarning($"Event Data Target Object is me {eventData.TargetObject} {this}");
            switch (eventData.EventType)
            {
                case FP_UIEventType.PointerDown:
                    PointerDown(eventData.UnityPointerEventData);
                    break;
                case FP_UIEventType.PointerUp:
                    PointerUp(eventData.UnityPointerEventData);
                    break;
                case FP_UIEventType.Drag:
                    PointerDrag(eventData.UnityPointerEventData);
                    break;
            }
        }
        else
        {
            //it's not me
            Debug.LogWarning($"Event Data Target Object is NOT me {eventData.TargetObject} {this}");
        }
    }

        public void PointerDown( PointerEventData eventData)
    {
        Debug.LogWarning($"Pointer down!-Cut");
        if(!ToolIsCurrent)
        {
            return;
        }
        //following maybe needed and adjusted to Cut format this was apart of FPmeasureTool2d but slashed out
        //Debug.LogWarning($"Passed Tool is Current");
        //need to convert the eventData.Position to the RectTransform space of the canvas

        //RectTransformUtility.ScreenPointToLocalPointInRectangle(measurementParentSpace,eventData.position,ToolCamera, out Vector2 localPoint);
        //Debug.LogWarning($"Event Data position: {eventData.position}, converted point {localPoint}");
        //bool inRectangle = RectTransformUtility.RectangleContainsScreenPoint(measurementParentSpace, eventData.position, ToolCamera);

        if (RectTransformUtility.RectangleContainsScreenPoint(CutParentSpace, eventData.position, ToolCamera))
        {
            if (StartTool())
            {
                Vector2 screenPosition = eventData.position;
                CutstartPosition = ScreenToRelativeRectPosition(screenPosition, CutParentSpace);

                OnCutToolStarting.Invoke();
                var spawnedItem = Instantiate(toolData.CutPointPrefab, CutParentSpace);
                if (spawnedItem.GetComponent<FP_CutLine_AC>() != null)
                {
                    currentActiveLine = spawnedItem.GetComponent<FP_CutLine_AC>();
                    currentActiveLine.SetupCutLine(this, CutParentSpace, canvasRect, ToolCamera, toolData.CutMeasurementFontSetting, lineSortOrderCounter);
                    currentActiveLine.DropFirstPoint(CutstartPosition);
                    currentActiveLine.DropSecondPoint(CutstartPosition);
                    UpdateTextFormat(0);
                    currentActiveLine.UpdateTextLocation(CutstartPosition);
                    allCutLines.Add(currentActiveLine);
                    // I think actual cutting mechanic code should go here
                    URP_Shadergraph_Cutmat.SetFloat(Cutmat, Cut_per = -5.36f);  // Maybe cutmeasurmentPrefix isntead of URP_Shadergraph_Cutmat? 
                                                                                // Instantiate<thisgameobject>(pipe).setFloat(Cutmat, Cut_per = [Range(-1, 1)]; //based on cut position create something to do this.
                    Debug.LogWarning($"Cut line should have showed and material should have changed.");
                }
                else
                { 
                    Destroy(spawnedItem);
                    Debug.LogError($"missing FP_CutInidcator Line component");
                    DeactivateTool();
                }
            }
        }
    }

    /// <summary>
    /// use this somehwere, 
    ///  GameObject obj = Instantiate(pipe);

    // Assuming pipe has a Renderer and we're modifying a material property
  //  Renderer rend = obj.GetComponent<Renderer>();

    // Get world position of pointer
 //   Vector3 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);

    // Calculate local position within the object
   // Vector3 localPoint = obj.transform.InverseTransformPoint(worldPoint);

    // Assuming you want to map local x-position across the object's width to a range of -1 to 1
  //  float objectWidth = rend.bounds.size.x;
  //  float clampedX = Mathf.Clamp(localPoint.x, -objectWidth / 2, objectWidth / 2);
   // float normalizedX = (clampedX / (objectWidth / 2)); // Gives value from -1 to 1

    // Set shader/material float property
 //   if (rend.material.HasProperty("_Cutmat"))
  //  {
  //      rend.material.SetFloat("_Cutmat", normalizedX); // _Cutmat is the shader property name
   // }
/// </summary>
/// <param name="eventData"></param>




//may need some sort of on active use call in here based on unity debug log.
public void PointerDrag(PointerEventData eventData)
    {
        if(!ToolIsCurrent)
        {
            return;
        }
        if(UseTool())
        {
            CutendPosition = ScreenToRelativeRectPosition(eventData.position, CutParentSpace);
            if(currentActiveLine != null)
            {
                currentActiveLine.DropSecondPoint(CutendPosition);
            }
            UpdateMeasurementText();
        }
    }

    public void PointerUp(PointerEventData eventData)
    {
        Debug.Log($"On pointer up -Cut");
        if(!ToolIsCurrent)
        {
            return;
        }
        //Are we in position?
        if (RectTransformUtility.RectangleContainsScreenPoint(CutParentSpace, eventData.position, ToolCamera))
        {
            if (EndTool())
            {
                CutendPosition = ScreenToRelativeRectPosition(eventData.position, CutParentSpace);
                if (currentActiveLine != null)
                {
                    currentActiveLine.DropSecondPoint(CutendPosition);
                }
                UpdateMeasurementText();
                OnCutToolEnding.Invoke();
                DeactivateTool();
                lineSortOrderCounter++;
                if (ConstantCutmeasurement)
                {
                    //reset cut line to start a new one without having to push the button
                    Initialize(toolData);
                    ActivateTool();
                }
            }
        }
        else
        {
            //destroy it
            DeactivateTool() ;
            if (currentActiveLine != null)
            {
                Destroy(currentActiveLine.gameObject);
            }
            if (ConstantCutmeasurement)
            {

            Initialize(toolData);
            ActivateTool();
            }
        }
    }

    public override bool DeactivateTool()
    {
        if (base.DeactivateTool())
        {
            ToolIsCurrent = false;
            OnCutToolDeactivation.Invoke();
            return true;
        }
        return false;
    }


    public void ResetVisuals()
    {
        DeactivateResetLinesUI();
    }
    /// <summary>
    /// Returns a screen position coordinate based on the canvas already assigned in the inspector
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    /// 
    protected Vector2 ScreenToCanvasPosition(Vector2 screenposition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect.GetComponent<RectTransform>(), screenposition, ToolCamera, out Vector2 canvasPos);
        return canvasPos;
    }
    /// <summary>
    /// Returns a Screen Position Coordinate based on the rect transform passed in
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <param name="rect"></param>
    /// <returns></returns>
    /// 
    protected Vector2 ScreenToRelativeRectPosition(Vector2 screenPosition, RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPosition, ToolCamera, out Vector2 canvasPos);
        return canvasPos;
    }

    protected void UpdateMeasurementText()
    {
        float distance = Vector2.Distance(CutstartPosition, CutendPosition);
        //format text
        if (currentActiveLine != null)
        {
            UpdateTextFormat(distance);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="distance">incoming pixel coordinate value</param>
    /// 
     protected void UpdateTextFormat(float distance)
    {
        if (currentActiveLine != null)
        {
            var unitReturn = FP_UtilityData.ReturnUnitByPixels(canvasScaler.referencePixelsPerUnit, distance, toolData.measurementUnits);
            if (unitReturn.Item1)
            {
                var formatteddistance = unitReturn.Item2.ToString($"F{toolData.measurementPrecision}");
                currentActiveLine.UpdateTextInformation($"{toolData.measurementPrefix} {formatteddistance} {toolData.measurementUnits}");
            }
            else
            {
                Debug.LogWarning($"Failed to convert the distance {distance} to the correct measurement system {toolData.measurementUnits}");
                currentActiveLine.UpdateTextInformation($"{toolData.measurementPrefix} {distance} pixels");
            }

            Vector2 midpoint = (CutstartPosition + CutendPosition) * 0.5f;
            Vector2 direction = (CutendPosition - CutstartPosition).normalized;
            // Perpendicular vector (rotate 90 degrees CCW)
            Vector2 perpendicular = new Vector2(-direction.y, direction.x);
            // Apply offset relative to perpendicular direction
            Vector2 offset = perpendicular * toolData.measurementLabelOffsetPixels.y + direction * toolData.measurementLabelOffsetPixels.x;
            Vector2 labelPosition = midpoint + offset;
            currentActiveLine.UpdateTextLocation(labelPosition);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            currentActiveLine.SetTextRotation(angle);
        }
    }
}
