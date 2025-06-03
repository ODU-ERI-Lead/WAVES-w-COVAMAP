using UnityEngine;
using UnityEngine.UI;

public class Partspawnerscript : MonoBehaviour
{
    public Button Elbow;
    public Button Valve;
    public Button Pipe;
    public Button FemaleAdapter;
    public Button MaleAdapter;

    public GameObject[] PartsTospawn;
    public Transform SpawnPoint;
    public Transform LeftSpawnEdge;
    public Transform RightSpawnEdge;
    public int HorizontalBinCount = 10;
    public int ForwardBinCount = 5;
    protected int currentIndex=0;
    protected int currentIndexDepth = 0;
    public bool TestHorizontalChange;

    public void SpawnElbow()
    {
        //someValue = condition ? newValue : someOtherValue;
        Instantiate(PartsTospawn[0], TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);
    }

    public void SpawnValve()
    {
        Instantiate(PartsTospawn[1], TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);  
    }

    public void SpawnPipe()
    {
        Instantiate(PartsTospawn[2], TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);  
    }

    public void SpawnFemaleAdapter()
    {
        Instantiate(PartsTospawn[3], TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);
    }

    public void SpawnMaleAdapter()
    {
        Instantiate(PartsTospawn[4], TestHorizontalChange ? GetSpawnLocation() : SpawnPoint.position, Quaternion.identity);
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
        SpawnPoint.position = LeftSpawnEdge.position + new Vector3(xChange, 0, zChange);
        return SpawnPoint.position;
    }
}
