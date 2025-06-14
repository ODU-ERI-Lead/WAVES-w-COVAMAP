using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages a general blueprint concept for the PipeFitter game.
/// </summary>
public class PFBluePrint : MonoBehaviour
{
    [Space]
    [Header("Workbench Blue Print Panel")]
    public Camera BPCameraOverlay;
    public Canvas BPCanvas;
    public RectTransform BluePrintPanel;
    public Button BluePrintReturnButton;
    public Button BluePrintZoomInButton;
    public Button BluePrintZoomOutButton;

    public int MaxZoomInt = 5;
    protected int zoomStartValue = 0;
    public float ZoomPanelRatioX = 50;
    private Vector2 originalAnchorMin;
    private Vector2 originalAnchorMax;

    private float zoomPanelRatioY { get { return ZoomPanelRatioX * 0.5625f; } } // 16:9 aspect ratio

    public void OnEnable()
    {
        originalAnchorMin = BluePrintPanel.anchorMin;
        originalAnchorMax = BluePrintPanel.anchorMax;
    }
    /// <summary>
    /// Openens the blue print
    /// </summary>
    public void UIBluePrintOpen()
    {
        if (BPCanvas != null)
        {
            BPCanvas.gameObject.SetActive(true);
            if (BluePrintPanel != null)
            {
                BluePrintPanel.gameObject.SetActive(true);
            }
            zoomStartValue = 0;
            BluePrintSetCameraByZoom();
            CheckBluePrintZoomButtons();
        }
    }
    /// <summary>
    /// Closes the blue print
    /// </summary>
    public void UIBluePrintClosed()
    {
        if (BPCanvas != null)
        {
            zoomStartValue = 0;
            BluePrintSetCameraByZoom();
            if (BluePrintPanel != null)
            {
                BluePrintPanel.gameObject.SetActive(false);
            }
            BPCanvas.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// UI button to zoom in from the blueprint work assembly
    /// </summary>
    public void UIBluePrintZoomIn()
    {
        zoomStartValue++;
        if (zoomStartValue > MaxZoomInt)
        {
            zoomStartValue = MaxZoomInt;
        }
        BluePrintSetCameraByZoom();
        CheckBluePrintZoomButtons();
    }

    /// <summary>
    /// UI button to zoom out from the blueprint work assembly
    /// </summary>
    public void UIBluePrintZoomOut()
    {
        zoomStartValue--;
        if (zoomStartValue < 0)
        {
            zoomStartValue = 0;
        }
        BluePrintSetCameraByZoom();
        CheckBluePrintZoomButtons();
    }
    /// <summary>
    /// Update UI Zoom Buttons based on where we are in the zoom depth
    /// </summary>
    private void CheckBluePrintZoomButtons()
    {
        if (BluePrintZoomOutButton != null)
        {
            if (zoomStartValue > 0)
            {
                BluePrintZoomOutButton.interactable = true;
            }
            else
            {
                BluePrintZoomOutButton.interactable = false;
            }
        }
        if (BluePrintZoomInButton != null)
        {
            if(zoomStartValue < MaxZoomInt)
            {
                BluePrintZoomInButton.interactable = true;
            }
            else
            {
                BluePrintZoomInButton.interactable = false;
            }
        }
    }
    protected void BluePrintSetCameraByZoom()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float zoomedWidthPixel = screenWidth + (zoomStartValue * ZoomPanelRatioX);
        float zoomedHeightPixel = screenHeight + (zoomStartValue * zoomPanelRatioY);

        float widthNorm = zoomedWidthPixel / screenWidth;
        float heightNorm = zoomedHeightPixel / screenHeight;

        float anchorCenterX = (originalAnchorMin.x + originalAnchorMax.x) / 2f;
        float anchorCenterY = (originalAnchorMin.y + originalAnchorMax.y) / 2f;

        float newAnchorWidth = widthNorm;
        float newAnchorHeight = heightNorm;

        BluePrintPanel.anchorMin = new Vector2(anchorCenterX - newAnchorWidth / 2f, anchorCenterY - newAnchorHeight / 2f);
        BluePrintPanel.anchorMax = new Vector2(anchorCenterX + newAnchorWidth / 2f, anchorCenterY + newAnchorHeight / 2f);
    }
}
