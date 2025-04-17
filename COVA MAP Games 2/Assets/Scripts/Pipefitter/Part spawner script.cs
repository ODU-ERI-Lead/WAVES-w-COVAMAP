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



    public void SpawnElbow()
    {
        Instantiate(PartsTospawn[0],SpawnPoint.position, Quaternion.identity);
    }

    public void SpawnValve()
    {

    }

    public void SpawnPipe()
    {

    }

    public void SpawnFemaleAdapter()
    {

    }

    public void SpawnMaleAdapter()
    { 
    
    }
}
