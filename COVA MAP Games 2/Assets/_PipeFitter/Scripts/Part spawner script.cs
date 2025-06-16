using System;
using UnityEngine;
using UnityEngine.UI;


public class Partspawnerscript : MonoBehaviour
{
    public Button Elbow;
    public Button Valve;
    public Button Pipe;
    public Button FemaleAdapter;
    public Button MaleAdapter;
    [Space]
    [Header("Parts to Spawn")]
    public GameObject ElbowPrefab;
    public GameObject ValvePrefab;
    public GameObject PipePrefab;
    public GameObject FemaleAdapterPrefab;
    public GameObject MaleAdapterPrefab;
    [Space]
   
    public Transform SpawnPoint;

    public bool TestHorizontalChange;
   
    public delegate void PartSpawned(GameObject thePart);
    public event PartSpawned ElbowSpawned;
    public event PartSpawned ValveSpawned;
    public event PartSpawned MAdapterSpawned;
    public event PartSpawned FAdapterSpawned;
    public event PartSpawned PipeSpawned;
    public GridSpawnSystem MyGridSystem;
    protected GameObject lastSpawnedItem;
    public GameObject ReturnLastSpawnedItem { get => lastSpawnedItem; }

    public void Start()
    {
        /*
        partsTospawn = new GameObject[5];
        partsTospawn[0] = ElbowPrefab;
        partsTospawn[1] = ValvePrefab;
        partsTospawn[2] = PipePrefab;
        partsTospawn[3] = FemaleAdapterPrefab;
        partsTospawn[4] = MaleAdapterPrefab;
        */
    }
    public void SpawnElbow()
    {
        //someValue = condition ? newValue : someOtherValue;
        lastSpawnedItem = Instantiate(ElbowPrefab, TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);
        ElbowSpawned?.Invoke(lastSpawnedItem);
    }

    public void SpawnValve()
    {
        lastSpawnedItem = Instantiate(ValvePrefab, TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);
        ValveSpawned?.Invoke(lastSpawnedItem);
    }

    public void SpawnPipe()
    {
        lastSpawnedItem = Instantiate(PipePrefab, TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);
        PipeSpawned?.Invoke(lastSpawnedItem);
    }

    public void SpawnFemaleAdapter()
    {
        lastSpawnedItem = Instantiate(FemaleAdapterPrefab, TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);
        FAdapterSpawned?.Invoke(lastSpawnedItem);
    }

    public void SpawnMaleAdapter()
    {
        lastSpawnedItem = Instantiate(MaleAdapterPrefab, TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);
        MAdapterSpawned?.Invoke(lastSpawnedItem);
    }
    [Obsolete("We shouldn't use this anymore")]
    public Vector3 GetSpawnLocation()
    {
        return SpawnPoint.position;
        
    }
}
