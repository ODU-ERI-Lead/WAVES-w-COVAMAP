using FuzzPhyte.Tools;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using FuzzPhyte.Utility;
/// <summary>
/// Used to store information on deployed 'cut line'
/// Should be part of a root transform prefab
/// </summary>

public class FP_CutLine_AC : MonoBehaviour, IFPToolListener<FP_CutToolData>
{
    protected FP_Tool<FP_CutToolData> cutTool;
    public RectTransform FirstPoint;
    public RectTransform secondPoint;
    public TextMeshProUGUI MeasurementText;
    public LineRenderer LineRenderer;
    protected RectTransform workSpace;
    protected Camera mycamera;
    protected Canvas Canvas;

    /// <summary>
    /// Setup the prefab, we need the data and the work space our 2D tool will be in
    /// </summary>
    /// <param name="cutTool">The data</param>
    /// <param name="rectPanel">Work space for the 2D tool</param>
    /// 

    public void SetupCutLine(FP_Tool<FP_CutToolData> theTool, RectTransform rectPanel, Canvas mainCanvas, Camera mainCamera, FontSetting fontdetails, int lineRenderOrder = 10)
    {
        workSpace = rectPanel;
        mycamera = mainCamera;
        Canvas = mainCanvas;
        cutTool = theTool;
        LineRenderer.positionCount = 2;
        //testing local space
        LineRenderer.useWorldSpace = false;
        LineRenderer.sortingOrder = lineRenderOrder;
        Material cachedMat = theTool.ToolData.IndicatorMat;
        cachedMat.color = theTool.ToolData.CutIndicator;

        List<Material> materials = new List<Material>();
        materials.Add(cachedMat);
        LineRenderer.SetMaterials(materials);
        LineRenderer.startColor = theTool.ToolData.CutIndicator;
        LineRenderer.endColor = theTool.ToolData.CutIndicator;
        LineRenderer.startWidth = theTool.ToolData.CutIndicWidth;
        FP_UtilityData.ApplyFontSetting(MeasurementText, fontdetails);
        //now to listen in
        cutTool.OnActivated += OnToolActivated;
        cutTool.OnStarting += OnToolStarting;
        cutTool.OnActiveUse += OnToolInActiveUse;
        cutTool.OnEnding += OnToolEnding;
        cutTool.OnDeactivated += OnToolDeactivated;
    }

    public virtual void DropFirstPoint(Vector2 screenPosition)
    {
        //moving in slashed out from FP_measureline may need to be converted etc.
        //convert the pixel space to world space
        /*
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        parentCanvas.transform as RectTransform,
        Input.mousePosition, parentCanvas.worldCamera,
        out movePos);
    Vector3 positionToReturn = parentCanvas.transform.TransformPoint(movePos);
    positionToReturn.z = parentCanvas.transform.position.z - 0.01f;
        */
        //Vector2 aPosition = RectTransformUtility.ScreenPointToLocalPointInRectangle(workSpace, screenPosition, myCamera, out Vector2 somePos) ? somePos : Vector2.zero;
        FirstPoint.localPosition = screenPosition;
        FirstPoint.gameObject.SetActive(true);
        secondPoint.gameObject.SetActive(true);
        var depthAdj = new Vector3 (screenPosition.x, screenPosition.y, -1f);
        //Vector3 worldPos = workSpace.transform.TransformPoint(aPosition);
        LineRenderer.SetPosition(0, depthAdj);  
        LineRenderer.SetPosition(1, depthAdj);  
    }

    public virtual void DropSecondPoint(Vector2 screenPosition)
    {
        secondPoint.localPosition = screenPosition;
        var depthAdj = new Vector3(screenPosition.x, screenPosition.y, -1f);
        //Vector2 aPosition = RectTransformUtility.ScreenPointToLocalPointInRectangle(workSpace, screenPosition, myCamera, out Vector2 somePos) ? somePos : Vector2.zero;
        //Vector3 worldPos = workSpace.transform.TransformPoint(aPosition);
        LineRenderer.SetPosition(1, depthAdj);
    }

    public void OnDestroy()
    {
        if(cutTool != null)
        {
            cutTool.OnActivated -= OnToolActivated;
            cutTool.OnStarting -= OnToolStarting;   
            cutTool.OnActiveUse -= OnToolInActiveUse;
            cutTool.OnEnding -= OnToolEnding;
            cutTool.OnDeactivated -= OnToolDeactivated;
        }
    }







    public void OnToolActivated(FP_Tool<FP_CutToolData> tool)
        {
            if (tool != cutTool) return;
        }
        public void OnToolStarting(FP_Tool<FP_CutToolData> tool)
        {
            if (tool != cutTool) return;
        }
        public void OnToolInActiveUse(FP_Tool<FP_CutToolData> tool)
        {
            if (tool != cutTool) return;
        }
        public void OnToolEnding(FP_Tool<FP_CutToolData> tool)
        {
            if (tool != cutTool) return;
        }
        public void OnToolDeactivated(FP_Tool<FP_CutToolData> tool)
        {
            if (tool != cutTool) return;
        }
        public void UpdateTextInformation(string text)
        {
            MeasurementText.text = text;
        }
        public void UpdateTextLocation(Vector2 coordinate)
        {
            MeasurementText.rectTransform.anchoredPosition = coordinate;
        }
        public void SetTextRotation(float angle)
        {
            if (angle > 90f || angle < -90f)
            {
                angle += 180f;
            }
            MeasurementText.rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

