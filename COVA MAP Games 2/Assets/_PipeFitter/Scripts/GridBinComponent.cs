using UnityEngine;

/// <summary>
/// used to hold information about a grid item
/// </summary>
public class GridBinComponent : MonoBehaviour
{
    public int XIndex { get; private set; }
    public int ZIndex { get; private set; }
    public GridSpawnSystem Grid { get; private set; }
    protected Bounds myBounds;

    [SerializeField]
    private GameObject currentOccupant;

    public GameObject GridVisual;
    public void Initialize(GridSpawnSystem grid, int x, int z,Vector2 size)
    {
        Grid = grid;
        XIndex = x;
        ZIndex = z;
        //set our visual to match
        GridVisual.transform.localScale = new Vector3(size.x, size.x, size.y);
        myBounds = new Bounds(transform.position, new Vector3(size.x, size.x, size.y));
    }

    public bool IsOccupied()
    {
        return currentOccupant != null;
    }

    public void SetOccupant(GameObject obj)
    {
        currentOccupant = obj;
    }

    public void ClearOccupant()
    {
        currentOccupant = null;
    }

    public GameObject GetOccupant()
    {
        return currentOccupant;
    }
    public void FixedUpdate()
    {
        if (currentOccupant == null)
        {
            return;
            // Update the position of the occupant to match the bin's position
           
        }
        if (!myBounds.Contains(currentOccupant.transform.position))
        {
            ClearOccupant();
        }
    }
}
