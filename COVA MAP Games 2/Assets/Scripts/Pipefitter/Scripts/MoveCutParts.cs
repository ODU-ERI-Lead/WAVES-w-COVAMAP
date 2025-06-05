using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class MoveCutParts : MonoBehaviour
{
    public Button SendtoAssemblyButton;
    [SerializeField] private GameObject CutPart1;
    [SerializeField] private GameObject CutPart2;
    public GameObject OtherPart;
    public Transform AssemblyAreaSpawn;
    //public Partspawnerscript PipePartSpawner;
    public Partspawnerscript OtherPartsSpawner;
    //private 



    public void OnEnable()
    {
        PipeFitterMouseCutter.RetrievePartsFromCut += GettingCutParts;
       
        if(OtherPartsSpawner != null)
        {
            OtherPartsSpawner.ElbowSpawned += NewPartAddedToBench;
            OtherPartsSpawner.PipeSpawned += NewPartAddedToBench;
        }
        SendtoAssemblyButton.onClick.AddListener(UIMovePartToHomeClicked);
        //need a way to listen in on other parts being spawned Elbow/valve/adapter(s)
    }

    public void OnDisable()
    {
        PipeFitterMouseCutter.RetrievePartsFromCut -= GettingCutParts;
        
        if (OtherPartsSpawner != null)
        {
            OtherPartsSpawner.ElbowSpawned -= NewPartAddedToBench;
            OtherPartsSpawner.PipeSpawned -= NewPartAddedToBench;
        }
        SendtoAssemblyButton.onClick.RemoveListener(UIMovePartToHomeClicked);
    }
    /// <summary>
    /// This needs to be referenced when we push the rotate button 
    /// </summary>
    /// <returns></returns>
    public List<GameObject> ReturnPartsToRotate()
    {
        List<GameObject> CurrentPartsInWorkBench = new List<GameObject>();

        if (CutPart1 != null && CutPart2 != null)
        {
            CurrentPartsInWorkBench.Add(CutPart1.gameObject);
            CurrentPartsInWorkBench.Add(CutPart2.gameObject);
            return CurrentPartsInWorkBench;
        }
        if (OtherPart != null)
        {
            //other part
            CurrentPartsInWorkBench.Add(OtherPart);
            return CurrentPartsInWorkBench;
        }
        return null;
    } 

    //Chache-ing cut objects
    public void GettingCutParts(GameObject CutPartLeft, GameObject CutPartRight)
    {
        CutPart1 = CutPartLeft;
        CutPart2 = CutPartRight;
        Debug.Log("Recieved Cut Gameobjects");
        Debug.Log("Cut Part 1" + CutPartLeft.name);
        Debug.Log("Cut Part 2" + CutPartRight.name);
        if (OtherPart != null)
        {
            Debug.LogWarning($"This was a long pipe that we cached... now we need to get rid of that cache");
            OtherPart=null;
        }
    }

    /// <summary>
    /// this will be called via the Home Button from the UI
    /// </summary>
    public void UIMovePartToHomeClicked()
    {
        if(CutPart1 != null && CutPart2 != null)
        {
            //long pipe
            PostCutMove();
            //
            return;
        }
        if (OtherPart != null)
        {
            //other part
            MovePart(OtherPart.transform, OtherPartsSpawner);
            OtherPart = null;
        }
    }

    protected void PostCutMove()
    {
        if (CutPart1 != null && CutPart2 != null)
        {
            if (AssemblyAreaSpawn != null)
            {
                Transform root1 = GetRoot(CutPart1.transform);
                Transform root2 = GetRoot(CutPart2.transform);
                MovePart(root1, OtherPartsSpawner);
                MovePart(root2, OtherPartsSpawner);
                CutPart1 = null;
                CutPart2 = null;
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
    /// <summary>
    /// Stub out ready to accept a part coming in from the other categories
    /// </summary>
    /// <param name="partAdded"></param>
    public void NewPartAddedToBench(GameObject partAdded)
    {
        OtherPart = partAdded;
    }
    /// <summary>
    /// Will move the part to a location defined by the spawner you give us
    /// </summary>
    /// <param name="partToMove"></param>
    /// <param name="spawner"></param>
    protected void MovePart(Transform partToMove, Partspawnerscript spawner)
    {
        AssemblyAreaSpawn.transform.position = spawner.GetSpawnLocation();
        partToMove.position = AssemblyAreaSpawn.position;
    }

    private Transform GetRoot(Transform t)
    {
        while (t.parent != null)
            t = t.parent;
        return t;
    }


}
