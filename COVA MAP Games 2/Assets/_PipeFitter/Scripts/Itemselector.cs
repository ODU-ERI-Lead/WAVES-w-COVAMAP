using UnityEngine;

public class Itemselector : MonoBehaviour
{
    public static GameObject lastSelected;
    public LayerMask SelectableLayer;
    private PipefitterpartData pipefitterpartData;
    private FinalInspectionScript FinalInspectionScript;
    public bool UseSelectorCaster = false;
    

    void Update()
    {
        if (!UseSelectorCaster)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, SelectableLayer))
            {
                GameObject clickedObject = hit.collider.transform.root.gameObject;
                lastSelected = clickedObject;
                Debug.Log("Selected: " + lastSelected.name);
                Debug.LogWarning("Clicked: " + hit.collider.name);
                Debug.LogWarning("Selected root: " + hit.collider.transform.root.name);
               // Debug.LogWarning("Part selected ID for data file"+ slot.expectedPartID)
            }
            
        }
    }




}
