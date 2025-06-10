using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AustinMouseInfo : MonoBehaviour
{
    public Mouse MouseInfo;
    public bool isMouseTrackingEnabled = false; // mouse tracking flag
    public Vector3 lastMousePosition;
    public Vector3 deltaMousePosition;
    public FuzzPhyte.Utility.FP_OrbitalRotation OrbitalRotationscript;
    public bool OrbitalRotationscript_ready;
    public Button OrbitalRotationButtonON;
    public Button OrbitalRotationButtonOFF;
    private Vector3 mousePos;

    public void Start()
    {
        OrbitalRotationButtonON.interactable = true;
        OrbitalRotationButtonOFF.interactable = false;
    }
    void Update()
    {
        if (isMouseTrackingEnabled)
        {
            mousePos = Input.mousePosition;

            if (OrbitalRotationscript_ready)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OrbitalRotationscript.Setup(mousePos);
                    OrbitalRotationscript.StartMotion();
                }

                if (Input.GetMouseButton(0))
                {
                    OrbitalRotationscript.OrbitalRotation(mousePos);
                }

                
            }
            deltaMousePosition = mousePos - lastMousePosition;


            lastMousePosition = mousePos;
        }


    }
    public void ButtonOnRotationToolActive()
    {
        OrbitalRotationButtonON.interactable = false;
        OrbitalRotationButtonOFF.interactable = true;
        OrbitalRotationscript.Setup(mousePos);
        OrbitalRotationscript_ready = true;
    }
    public void ButtonOFFRotationToolInactive()
    {
        OrbitalRotationButtonOFF.interactable = false;
        OrbitalRotationButtonON.interactable = true;
        OrbitalRotationscript.EndMotion();
        OrbitalRotationscript_ready = false;
    }
    public Vector3 GetMouseDelta()
    {
        return deltaMousePosition;
    }






}
//
