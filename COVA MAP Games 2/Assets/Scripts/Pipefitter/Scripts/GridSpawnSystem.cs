using UnityEngine;

public class GridSpawnSystem : MonoBehaviour
{
    public Transform bottomLeftCorner;
    public Transform topRightCorner;
    public GameObject binPrefab; // prefab with GridBinComponent attached
    public int binsX = 5;
    public int binsZ = 5;

    private GridBinComponent[,] bins;

    private float cellWidth;
    private float cellDepth;

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        Vector3 bottomLeft = bottomLeftCorner.position;
        Vector3 topRight = topRightCorner.position;

        float totalWidth = topRight.x - bottomLeft.x;
        float totalDepth = topRight.z - bottomLeft.z;

        cellWidth = totalWidth / binsX;
        cellDepth = totalDepth / binsZ;

        bins = new GridBinComponent[binsX, binsZ];

        for (int x = 0; x < binsX; x++)
        {
            for (int z = 0; z < binsZ; z++)
            {
                Vector3 spawnPos = new Vector3(
                    bottomLeft.x + (x + 0.5f) * cellWidth,
                    bottomLeft.y,
                    bottomLeft.z + (z + 0.5f) * cellDepth
                );

                GameObject binGO = Instantiate(binPrefab, spawnPos, Quaternion.identity, this.transform);
                GridBinComponent binComponent = binGO.GetComponent<GridBinComponent>();
                binComponent.Initialize(this, x, z,new Vector2(cellWidth,cellDepth));
                bins[x, z] = binComponent;
            }
        }
    }

    public GridBinComponent GetNextAvailableBin()
    {
        for (int x = 0; x < binsX; x++)
        {
            for (int z = 0; z < binsZ; z++)
            {
                if (!bins[x, z].IsOccupied())
                    return bins[x, z];
            }
        }

        return null;
    }

    public GridBinComponent GetBinAt(int x, int z)
    {
        if (x >= 0 && x < binsX && z >= 0 && z < binsZ)
            return bins[x, z];

        return null;
    }

    public (bool,Vector3) AssignGridBinComponent(GameObject part)
    {
        var aLocation = GetNextAvailableBin();
        if (aLocation != null)
        {
            aLocation.SetOccupant(part);
            //part.transform.position = aLocation.transform.position;
            //part.transform.rotation = aLocation.transform.rotation;
            return (true, aLocation.transform.position);
        }
        else
        {
            Debug.LogWarning("No available grid bin found for the part.");
            return (false, Vector3.zero);
        }
    }
}
