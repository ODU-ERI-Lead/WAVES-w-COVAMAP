using System;
using UnityEngine;
using UnityEngine.UI;


public class MoveCutParts : MonoBehaviour
{
    public Button SendtoAssemblyButton;
    private GameObject CutPart1;
    private GameObject CutPart2;
    public Transform AssemblyAreaSpawn;
    //private 



    public void OnEnable()
    {
        PipeFitterMouseCutter.RetrievePartsFromCut += GettingCutParts;
        SendtoAssemblyButton.onClick.AddListener(PostCutMove);
    }

    public void OnDisable()
    {
        PipeFitterMouseCutter.RetrievePartsFromCut -= GettingCutParts;
        SendtoAssemblyButton.onClick.RemoveListener(PostCutMove);
    }



    //Chache-ing cut objects
    public void GettingCutParts(GameObject CutPartLeft, GameObject CutPartRight)
    {
        CutPart1 = CutPartLeft;
        CutPart2 = CutPartRight;
        Debug.Log("Recieved Cut Gameobjects");
        Debug.Log("Cut Part 1" +  CutPartLeft.name);
        Debug.Log("Cut Part 2" + CutPartRight.name);
    }

    public void PostCutMove()
    {
        if (CutPart1 != null && CutPart2 != null)
        {
            if (AssemblyAreaSpawn != null)
            {
                // getting roots to help keep things attached.
                Transform root1 = GetRoot(CutPart1.transform);
                Transform root2 = GetRoot(CutPart2.transform);
                //sending left to assembly area
                CutPart1.transform.position = AssemblyAreaSpawn.position;
                root1.position = AssemblyAreaSpawn.position;
                //sending right to assembly area may want to just not include this though.
                CutPart2.transform.position = AssemblyAreaSpawn.position;
                root2.position = AssemblyAreaSpawn.position;
             }
            else
            {
                Debug.LogWarning(" Aseembly transform not set");
            }
        }
        else
        {
            Debug.LogWarning("Cut parts not set from Event. Did you call it right or are there other parts missing, or nothing listening?");
        }
    }

    private Transform GetRoot(Transform t)
    {
        while (t.parent != null)
            t = t.parent;
        return t;
    }


}
