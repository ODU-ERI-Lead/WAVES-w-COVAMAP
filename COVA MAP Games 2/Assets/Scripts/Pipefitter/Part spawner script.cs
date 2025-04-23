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
        Instantiate(PartsTospawn[1],SpawnPoint.position, Quaternion.identity);  
    }

    public void SpawnPipe()
    {
        Instantiate(PartsTospawn[2],SpawnPoint.position, Quaternion.identity);  
    }

    public void SpawnFemaleAdapter()
    {
        Instantiate(PartsTospawn[3],SpawnPoint.position, Quaternion.identity);
    }

    public void SpawnMaleAdapter()
    {
        Instantiate(PartsTospawn[4],SpawnPoint.position, Quaternion.identity);
    }
}
