using FuzzPhyte.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class hazardRaycast : MonoBehaviour
{
    public Camera MyCam;
    public bool IsCasting=false;
    public Transform FakeMouse;
    public Canvas parentCanvas;
    public float zDist;
    public List<RectTransform> AllHazardColliders = new List<RectTransform>();
    //public RectTransform TestMousePosRect;
    //public bool HITTHERECT;
    public void Update()
    {
        if (!IsCasting)
        {
            return;
        }
       
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);

        Vector3 mousePos = parentCanvas.transform.TransformPoint(movePos);

        //Set fake mouse Cursor
        FakeMouse.position = mousePos;

        //Move the Object/Panel
        //transform.position = mousePos;
        //FakeMouse.localPosition = new Vector3(FakeMouse.localPosition.x, FakeMouse.localPosition.y, zDist);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastCheck(movePos);
        }
    }
    public void RaycastCheck(Vector2 screenPoint)
    {

        //assumes a mouse... touch wouldn't work
        Debug.Log($"Cast {screenPoint}");
        RectTransform foundMatch = null;
        for (int i = 0; i < AllHazardColliders.Count; i++) 
        { 
            var aHazard = AllHazardColliders[i];
            if (RectTransformUtility.RectangleContainsScreenPoint(aHazard,screenPoint, MyCam))
            {
                Debug.Log($"In the box!");
                var potential = aHazard.gameObject.GetComponent<hazardCollider>();
                if (potential != null)
                {
                    potential.OnClicked();
                    foundMatch = aHazard;
                    Debug.Log($"Found A match {potential.gameObject.name}");
                    //remove from list if you wanted to
                    break;
                }
            }
        }
        if (foundMatch!=null)
        {
            AllHazardColliders.Remove(foundMatch);
        }
        /*
        RaycastHit2D hit = Physics2D.Raycast(MyCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        Debug.DrawRay(MyCam.ScreenToWorldPoint(Input.mousePosition), hit.point, Color.cyan);
        Debug.Log($"Cast");
        if (hit.collider != null)
        {
            Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
            var potentialHazard = hit.collider.gameObject.GetComponent<hazardCollider>();
            if (potentialHazard!=null)
            {
                Debug.LogWarning($"I FOUND IT");
                potentialHazard.OnClicked();
            }
        }
        */
    }
}
