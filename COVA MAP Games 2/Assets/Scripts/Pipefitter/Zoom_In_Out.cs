using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Zoom_In_Out : MonoBehaviour, IPointerDownHandler, IDragHandler 
{
    public Button Zoom_In;
    public Button Zoom_Out;
    public float zoomDelta = 0.1f;

    public float MaxZoom;
    public float MinZoom;
    public RectTransform BPtoZoom;
    public Camera mainCamera;
    public Camera CutterCamera;
    public Vector2 pointerDownPos;
    public Vector2 startPos;
    public RectTransform BPrectTransform;

    public Camera GetMainCamera()
    {
        return mainCamera;
    }

    public Camera GetCutterCamera()
    { 
        return CutterCamera; 
    }

    public void OnZoom()
    {
        // float v = Mathf.Clamp(mainCamera + value, MinZoom, MaxZoom);
        Vector3 newScale = BPtoZoom.localScale + Vector3.one * zoomDelta;
        newScale = ClampScale(newScale);
        BPtoZoom.localScale = newScale;
        if (mainCamera.orthographicSize > MinZoom)
      mainCamera.orthographicSize -= zoomDelta;
      Debug.LogWarning("camera should be zooming in");
    }

    public void OnZoomOut()
    {
        Vector3 newScale = BPtoZoom.localScale - Vector3.one * zoomDelta;
        newScale = ClampScale(newScale);
        BPtoZoom.localScale = newScale;
        if (mainCamera.orthographicSize < MaxZoom)
      mainCamera.orthographicSize += zoomDelta;
      Debug.LogWarning("camera should be zooming out");

    }

    private Vector3 ClampScale(Vector3 scale)
    {
        float clamped = Mathf.Clamp(scale.x, MinZoom, MaxZoom);
        return new Vector3(clamped, clamped, 1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            BPrectTransform, eventData.position, eventData.pressEventCamera, out pointerDownPos);
        startPos = BPrectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPointerPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            BPrectTransform, eventData.position, eventData.pressEventCamera, out currentPointerPos);

        Vector2 offset = currentPointerPos - pointerDownPos;
        BPrectTransform.anchoredPosition = startPos + offset;
    }


}
