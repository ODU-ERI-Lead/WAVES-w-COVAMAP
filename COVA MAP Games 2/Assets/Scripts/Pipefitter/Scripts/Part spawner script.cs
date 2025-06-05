using UnityEngine;
using UnityEngine.UI;

public enum SpawnAreaType
{
    NA=0,
    Shelf=1,
    Bench=2,
    Ground=3
}
public class Partspawnerscript : MonoBehaviour
{
    public Button Elbow;
    public Button Valve;
    public Button Pipe;
    public Button FemaleAdapter;
    public Button MaleAdapter;

    public GameObject[] PartsTospawn;
    public Transform SpawnPoint;
    public Transform BinSpawnPoint;
    public Transform LeftSpawnEdge;
    public Transform RightSpawnEdge;
    public int HorizontalBinCount = 10;
    public int ForwardBinCount = 5;
    protected int currentIndex=0;
    protected int currentIndexDepth = 0;
    public bool TestHorizontalChange;
    public SpawnAreaType SpawnLocationType;
    public delegate void PartSpawned(GameObject thePart);
    public event PartSpawned ElbowSpawned;
    public event PartSpawned ValveSpawned;
    public event PartSpawned MAdapterSpawned;
    public event PartSpawned FAdapterSpawned;
    public event PartSpawned PipeSpawned;


    public void SpawnElbow()
    {
        //someValue = condition ? newValue : someOtherValue;
        var item = Instantiate(PartsTospawn[0], TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);
        ElbowSpawned?.Invoke(item);
    }

    public void SpawnValve()
    {
        var item = Instantiate(PartsTospawn[1], TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);
        ValveSpawned?.Invoke(item);
    }

    public void SpawnPipe()
    {
        var item = Instantiate(PartsTospawn[2], TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);
        PipeSpawned?.Invoke(item);
    }

    public void SpawnFemaleAdapter()
    {
        var item = Instantiate(PartsTospawn[3], TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);
        FAdapterSpawned?.Invoke(item);
    }

    public void SpawnMaleAdapter()
    {
        var item = Instantiate(PartsTospawn[4], TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);
        MAdapterSpawned?.Invoke(item);
    }
    public Vector3 GetSpawnLocation()
    {
        var dZ = Mathf.Abs(LeftSpawnEdge.transform.position.z-RightSpawnEdge.transform.position.z);
        var binDepth = dZ / ForwardBinCount;
        var binDepthMid = binDepth * 0.5f;
        var d = Vector3.Distance(LeftSpawnEdge.position, RightSpawnEdge.position);
        float zChange = binDepthMid + (currentIndexDepth * binDepth);
        var binSizeWidth = d / HorizontalBinCount;
        var binSizeMid = binSizeWidth * 0.5f;
        float xChange = binSizeMid + (currentIndex * binSizeWidth);
        if (currentIndex < HorizontalBinCount)
        {
            currentIndex++;
        }
        else
        {
            currentIndexDepth++;
            currentIndex = 0;
        }
        if(currentIndexDepth > ForwardBinCount)
        {
            currentIndexDepth = 0;
        }
        BinSpawnPoint.position = LeftSpawnEdge.position + new Vector3(xChange, 0, zChange);
        return BinSpawnPoint.position;
    }
}
