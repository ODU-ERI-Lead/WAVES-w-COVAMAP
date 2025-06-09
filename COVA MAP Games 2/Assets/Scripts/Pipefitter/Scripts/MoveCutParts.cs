using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FuzzPhyte.Tools.Connections;
using VInspector.Libs;

public class MoveCutParts : MonoBehaviour
{
    public Button SendtoAssemblyButton;
    [SerializeField] private GameObject CutPart1;
    [SerializeField] private GameObject CutPart2;
    public GameObject OtherPart;
    public Transform AssemblyAreaSpawn;
    //public Partspawnerscript PipePartSpawner;
    public Partspawnerscript OtherPartsSpawner;
    [Tooltip("We are tracking if a part is on the bench to remove it if we go home without sending it")]
    [SerializeField]
    protected bool partsOnBench = false;


    public void OnEnable()
    {
        PipeFitterMouseCutter.RetrievePartsFromCut += GettingCutParts;
       
        if(OtherPartsSpawner != null)
        {
            OtherPartsSpawner.ElbowSpawned += NewPartAddedToBench;
            OtherPartsSpawner.PipeSpawned += NewPartAddedToBench;
            OtherPartsSpawner.MAdapterSpawned += NewPartAddedToBench;
            OtherPartsSpawner.ValveSpawned += NewPartAddedToBench;
            OtherPartsSpawner.FAdapterSpawned += NewPartAddedToBench;
        }
        partsOnBench = false;
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
            OtherPartsSpawner.MAdapterSpawned -= NewPartAddedToBench;
            OtherPartsSpawner.ValveSpawned -= NewPartAddedToBench;
            OtherPartsSpawner.FAdapterSpawned -= NewPartAddedToBench;
        }
        if (partsOnBench)
        {
            //destroy here?
            if (CutPart1 != null && CutPart2 != null)
            {
                //long pipe destroy both parts
                Destroy(CutPart1);
                Destroy(CutPart2);
            }
            else
            {
                if (OtherPart != null)
                {
                    //other part
                    Destroy(OtherPart);       
                }
            }
        }
        partsOnBench = false;
        CutPart1 = null;
        CutPart2 = null;
        OtherPart = null;
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
            partsOnBench = false;
            return;
        }
        if (OtherPart != null)
        {
            //other part
            MovePart(OtherPart.transform, OtherPartsSpawner);
            partsOnBench = false;
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
        partsOnBench = true;
    }
    /// <summary>
    /// Will move the part to a location defined by the spawner you give us
    /// </summary>
    /// <param name="partToMove"></param>
    /// <param name="spawner"></param>
    protected void MovePart(Transform partToMove, Partspawnerscript spawner)
    {
        //john using new grid system
        if (spawner.MyGridSystem != null)
        {
            (bool amAbleToMove, Vector3 movedLocation) = spawner.MyGridSystem.AssignGridBinComponent(partToMove.gameObject);
            if (amAbleToMove)
            {
                partToMove.position = movedLocation;
                StartCoroutine(DelayClearConnectionPart(partToMove));
            }
            else
            {
                Debug.LogWarning($"Could not move {partToMove.name} to the grid system, no available bin found.");
                return;
            }
        }
        else
        {
            Debug.LogError($"Missing a grid System referenced for the spawner");
            return;
        }
        /*
            AssemblyAreaSpawn.transform.position = spawner.GetSpawnLocation();
        partToMove.position = AssemblyAreaSpawn.position;
        //lets check to see if we have a random connection setup and null it out
        if (partToMove.GetComponent<ConnectionPart>() != null)
        {
            StartCoroutine(DelayClearConnectionPart(partToMove));
        }
        */
    }
    System.Collections.IEnumerator DelayClearConnectionPart(Transform part)
    {
        yield return new WaitForEndOfFrame();
        ConnectionPart connectionPart = part.GetComponent<ConnectionPart>();
        for (int i = 0; i < connectionPart.MyConnectionPoints.Count; i++)
        {
            var aPoint = connectionPart.MyConnectionPoints[i];
            aPoint.ForceOtherClearConnection();
        }
    }

    private Transform GetRoot(Transform t)
    {
        while (t.parent != null)
            t = t.parent;
        return t;
    }
}
