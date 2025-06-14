using UnityEngine;
using FuzzPhyte.Tools;
using FuzzPhyte.Utility;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections.Generic;
using PipeFitter.Assembly;
public class PFHangerPlacementTool : FP_Tool<PFHangerData>, IFPUIEventListener<FP_Tool<PFHangerData>>
{
    [SerializeField] protected RectTransform measurementParentSpace;
    public Transform HangerDecalParent;
    [Space]
    [Header("Parameters we are caching")]
    [SerializeField] protected Vector3 startPosition;
    [SerializeField] protected Vector3 lastEndPosition;
    public Vector3 ReturnLastEndPos { get => lastEndPosition; }
    protected Plane cachedPlane;
    [SerializeField] protected List<PFHangerSetup> hangerDecals = new List<PFHangerSetup>();
    [SerializeField] protected PFHangerSetup currentHanger;
    [Space]
    public UnityEvent OnHangerToolClickedDown;
    public UnityEvent OnHangerToolClickedRelease;
   
    [Header("Additional Details")]
    public Transform ForwardPlaneLocation;

    public override bool DeactivateTool()
    {
        if (base.DeactivateTool())
        {
            if (!LoopTool)
            {
                //we do want to turn off ToolIsCurrent
                ToolIsCurrent = false;
            }
            return true;
        }
        Debug.LogWarning($"Didn't deactivate the tool?");
        return false;
    }

    /// <summary>
    /// This just sets our state up for being ready to use the tool
    /// </summary>
    public override bool ActivateTool()
    {
        if (base.ActivateTool())
        {
            ToolIsCurrent = true;
            OnHangerToolClickedDown.Invoke();
            return true;
        }
        Debug.LogWarning($"Didn't activate the tool?");
        return false;
    }
    public override bool ForceDeactivateTool()
    {
        Debug.LogWarning($"Hanger Force Deactivated!");
        if (base.ForceDeactivateTool())
        {
            ToolIsCurrent = false;
            return true;
        }
        return false;
    }
    public override bool LockTool()
    {
        if (base.LockTool())
        {
            ToolIsCurrent = false;
            return true;
        }
        return false;
    }
    public override bool UnlockTool()
    {
        if (base.UnlockTool())
        {
            return true;
        }
        return false;
    }
    public void OnUIEvent(FP_UIEventData<FP_Tool<PFHangerData>> eventData)
    {
        //Debug.LogWarning($"OnUIEvent was processed {eventData.EventType} {eventData.AdditionalData} {this} {ToolIsCurrent}");
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

    public void PointerDown(PointerEventData eventData)
    {
        Debug.LogWarning($"Pointer down!");
        if (!ToolIsCurrent)
        {
            return;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(measurementParentSpace, eventData.position, ToolCamera))
        {
            if (StartTool())
            {
                Debug.Log($"Start Measuring...");
                //if we do we then want to cast into 3D space
                //activate the ray - fire it once
                //move transform to the world space position based on the mouse position relative to rect
                //Plane fPlane = new Plane(ToolCamera.transform.forward,ForwardPlaneLocation.position);
                cachedPlane = new Plane(ForwardPlaneLocation.transform.forward * -1, ForwardPlaneLocation.position);
                FP_UtilityData.DrawLinePlane(ForwardPlaneLocation.position, ForwardPlaneLocation.forward * -1f, Color.green, 2, 10);
                var PointData = FP_UtilityData.GetMouseWorldPositionOnPlane(ToolCamera, eventData.position, cachedPlane);
                var direction = (PointData.Item2 - ToolCamera.transform.position).normalized;
                Ray ray = new Ray(ToolCamera.transform.position, direction);
                Debug.LogWarning($"Ray: {ray.origin} | {ray.direction}");
                Debug.DrawRay(ray.origin, ray.direction * toolData.RaycastMax, FP_UtilityData.ReturnColorByStatus(SequenceStatus.Unlocked), 10f);
                Debug.DrawRay(PointData.Item2, Vector3.up, Color.red, 9f);
               
                if (PointData.Item1)
                {
                    startPosition = PointData.Item2;
                    lastEndPosition = PointData.Item2;
                    //endPosition = PointData.Item2;
                    var changerObject = GameObject.Instantiate(toolData.HangerDecalPrefab,startPosition,Quaternion.identity);
                    currentHanger = changerObject.GetComponent<PFHangerSetup>();
                    if (currentHanger != null)
                    {
                        hangerDecals.Add(currentHanger);
                        currentHanger.SetupHangerDecals();
                    }
                    //currentActiveLine = spawnLinePrefab.GetComponent<FP_MeasureLine3D>();
                   
                    UseTool();
                }
            }
        }
    }

    public void PointerDrag(PointerEventData eventData)
    {
        if (!ToolIsCurrent)
        {
            return;
        }

        if (UseTool())
        {
            Plane fPlane = new Plane(ToolCamera.transform.forward, ForwardPlaneLocation.position);
            var PointData = FP_UtilityData.GetMouseWorldPositionOnPlane(ToolCamera, eventData.position, fPlane);
            if (PointData.Item1 && currentHanger!=null)
            {
                //do some stuff if we are true and dragging
                currentHanger.transform.position = PointData.Item2;
            }
        }
    }

    public void PointerUp(PointerEventData eventData)
    {
        Debug.Log($"On Pointer up");
        if (!ToolIsCurrent)
        {
            return;
        }
        //are we in the position?
        if (RectTransformUtility.RectangleContainsScreenPoint(measurementParentSpace, eventData.position, ToolCamera))
        {
            if (EndTool())
            {
                //update location
                Plane fPlane = new Plane(ToolCamera.transform.forward, ForwardPlaneLocation.position);
                var PointData = FP_UtilityData.GetMouseWorldPositionOnPlane(ToolCamera, eventData.position, fPlane);
                if (PointData.Item1)
                {
                    //do some stuff on the end of the tool being ended
                    currentHanger.transform.position = PointData.Item2;
                    currentHanger.transform.SetParent(HangerDecalParent);
                    currentHanger.HangerUpSwapDecal();
                    lastEndPosition = PointData.Item2;
                }
                
                OnHangerToolClickedRelease.Invoke();
                DeactivateTool();
            }
        }
        else
        {
            DeactivateTool();
        }
        currentHanger = null;
    }

    public void ResetVisuals()
    {
        ForceDeactivateTool();
        //blast all the lines
        foreach (var decal in hangerDecals)
        {
            Destroy(decal.gameObject);
        }
        hangerDecals.Clear();
    }
    public override void DeactivateToolFromUI()
    {
        this.ForceDeactivateTool();
    }
    
}
