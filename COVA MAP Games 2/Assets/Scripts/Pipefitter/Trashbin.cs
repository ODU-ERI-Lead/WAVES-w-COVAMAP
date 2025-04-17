using UnityEngine;

public class Trashbin : MonoBehaviour
{
    public void DeleteLastSelected()
    {
        if (Itemselector.lastSelected != null)
        {
            Debug.Log("Deleting: " + Itemselector.lastSelected.name);
            Destroy(Itemselector.lastSelected);
            Itemselector.lastSelected = null; // Clear the reference
        }
        else
        {
            Debug.Log("No item selected to delete.");
        }
    }
}
