using UnityEngine;
using UnityEngine.UI;

public class Zoom_In_Out : MonoBehaviour
{
    public Button Zoom_In;
    public Button Zoom_Out;
    public float zoomDelta = 0.1f;

    public float MaxZoom;
    public float MinZoom;

    public Camera mainCamera;
    public Camera CutterCamera;

    public Camera GetMainCamera()
    {
        return mainCamera;
    }

    public Camera GetCutterCamera()
    { 
        return CutterCamera; 
    }

    public void OnZoom(float value, Camera mainCamera)
    {
      //  float v = Mathf.Clamp(mainCamera + value, MinZoom, MaxZoom);
    }

    public void OnZoomOut(float value)
    {

    }






}
